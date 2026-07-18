using ConvenienceStore.Application.Constants;
using ConvenienceStore.Application.Contracts.Persistence;
using ConvenienceStore.Application.DTOs.Inventory;
using ConvenienceStore.Application.Services.Interfaces;
using ConvenienceStore.Domain.Entities;
using System.Security.Claims;

namespace ConvenienceStore.Application.Services;

public class BranchInventoryService : IBranchInventoryService
{
    private readonly IGenericRepository<BranchInventory> _branchInventoryRepository;
    private readonly IGenericRepository<Product> _productRepository;
    private readonly IGenericRepository<Branch> _branchRepository;
    private readonly INotificationService _notificationService;
    private readonly IUnitOfWork _unitOfWork;

    public BranchInventoryService(
        IGenericRepository<BranchInventory> branchInventoryRepository,
        IGenericRepository<Product> productRepository,
        IGenericRepository<Branch> branchRepository,
        INotificationService notificationService,
        IUnitOfWork unitOfWork)
    {
        _branchInventoryRepository = branchInventoryRepository;
        _productRepository = productRepository;
        _branchRepository = branchRepository;
        _notificationService = notificationService;
        _unitOfWork = unitOfWork;
    }

    private int GetCurrentUserId(ClaimsPrincipal currentUser)
    {
        var userIdString = currentUser.FindFirstValue("sub") ?? currentUser.FindFirstValue(ClaimTypes.NameIdentifier);
        if (int.TryParse(userIdString, out var userId))
            return userId;
        throw new UnauthorizedAccessException("Invalid user token.");
    }

    private void ValidateBranchAccess(int branchId, ClaimsPrincipal currentUser)
    {
        var role = currentUser.FindFirstValue(ClaimTypes.Role);
        if (role == "Chain Director" || role == "System Admin")
            return; // Can access all branches

        var userBranchIdString = currentUser.FindFirstValue("BranchId");
        if (string.IsNullOrEmpty(userBranchIdString) || !int.TryParse(userBranchIdString, out var userBranchId) || userBranchId != branchId)
        {
            throw new UnauthorizedAccessException("You do not have access to this branch's inventory.");
        }
    }

    private string GetUserFullName(ClaimsPrincipal currentUser)
    {
        // Fallback to name claim or just User
        return currentUser.FindFirstValue("FullName") ?? currentUser.FindFirstValue(ClaimTypes.Name) ?? "System User";
    }

    public async Task<BranchInventoryListDto> GetBranchInventoryAsync(int branchId, string? search, int page, int pageSize, ClaimsPrincipal currentUser)
    {
        ValidateBranchAccess(branchId, currentUser);

        var query = await _branchInventoryRepository.GetAsync(bi => bi.BranchId == branchId);
        var allProducts = await _productRepository.GetAllAsync();

        var joined = query.Join(allProducts, 
            bi => bi.ProductId, 
            p => p.ProductId, 
            (bi, p) => new { bi, p });

        if (!string.IsNullOrEmpty(search))
        {
            var lowerSearch = search.ToLower();
            joined = joined.Where(x => 
                x.p.ProductName.ToLower().Contains(lowerSearch) || 
                (x.p.Barcode != null && x.p.Barcode.ToLower().Contains(lowerSearch)));
        }

        var totalCount = joined.Count();
        
        var branch = await _branchRepository.GetByIdAsync(branchId);
        var branchName = branch?.BranchName ?? "";

        var items = joined
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new BranchInventoryDto
            {
                BranchId = branchId,
                BranchName = branchName,
                ProductId = x.bi.ProductId,
                ProductName = x.p.ProductName,
                Barcode = x.p.Barcode,
                Quantity = x.bi.Quantity,
                LastUpdated = x.bi.LastUpdated
            })
            .ToList();

        return new BranchInventoryListDto
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<BranchInventoryDto?> GetBranchInventoryByProductAsync(int branchId, int productId, ClaimsPrincipal currentUser)
    {
        ValidateBranchAccess(branchId, currentUser);

        var inventory = (await _branchInventoryRepository.GetAsync(bi => bi.BranchId == branchId && bi.ProductId == productId)).FirstOrDefault();
        if (inventory == null) return null;

        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null) return null;

        var branch = await _branchRepository.GetByIdAsync(branchId);

        return new BranchInventoryDto
        {
            BranchId = branchId,
            BranchName = branch?.BranchName ?? "",
            ProductId = inventory.ProductId,
            ProductName = product.ProductName,
            Barcode = product.Barcode,
            Quantity = inventory.Quantity,
            LastUpdated = inventory.LastUpdated
        };
    }

    public async Task<IEnumerable<BranchInventoryDto>> GetLowStockByBranchAsync(int branchId, int threshold, ClaimsPrincipal currentUser)
    {
        ValidateBranchAccess(branchId, currentUser);

        var query = await _branchInventoryRepository.GetAsync(bi => bi.BranchId == branchId && bi.Quantity <= threshold);
        var allProducts = await _productRepository.GetAllAsync();
        var branch = await _branchRepository.GetByIdAsync(branchId);

        var joined = query.Join(allProducts, 
            bi => bi.ProductId, 
            p => p.ProductId, 
            (bi, p) => new BranchInventoryDto
            {
                BranchId = branchId,
                BranchName = branch?.BranchName ?? "",
                ProductId = bi.ProductId,
                ProductName = p.ProductName,
                Barcode = p.Barcode,
                Quantity = bi.Quantity,
                LastUpdated = bi.LastUpdated
            }).ToList();

        return joined;
    }

    public async Task<StockAdjustmentResultDto> AdjustStockAsync(int branchId, StockAdjustmentRequest request, ClaimsPrincipal currentUser)
    {
        ValidateBranchAccess(branchId, currentUser);

        var role = currentUser.FindFirstValue(ClaimTypes.Role);
        if (role != "Store Manager" && role != "Chain Director" && role != "System Admin")
        {
            throw new UnauthorizedAccessException("You do not have permission to adjust stock.");
        }

        if (request.Items == null || !request.Items.Any())
            throw new Exception("Adjustment request items cannot be empty.");

        var duplicates = request.Items.GroupBy(i => i.ProductId).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
        if (duplicates.Any())
            throw new Exception($"Duplicate product IDs found: {string.Join(", ", duplicates)}");

        var userId = GetCurrentUserId(currentUser);
        var userName = GetUserFullName(currentUser);
        var branch = await _branchRepository.GetByIdAsync(branchId);
        var branchName = branch?.BranchName ?? $"Unknown Branch ({branchId})";

        var result = new StockAdjustmentResultDto
        {
            BranchId = branchId,
            BranchName = branchName
        };

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            foreach (var item in request.Items)
            {
                if (item.ActualQuantity < 0)
                    throw new Exception($"ActualQuantity cannot be negative for product {item.ProductId}");

                if (!AdjustmentReasons.IsValid(item.Reason))
                    throw new Exception($"Invalid reason '{item.Reason}' for product {item.ProductId}");

                var inventory = (await _branchInventoryRepository.GetAsync(bi => bi.BranchId == branchId && bi.ProductId == item.ProductId)).FirstOrDefault();
                if (inventory == null)
                    throw new Exception($"Inventory record not found for product {item.ProductId} in branch {branchId}");

                var oldQuantity = inventory.Quantity;
                var delta = item.ActualQuantity - oldQuantity;

                if (delta == 0) continue; // No change

                inventory.Quantity = item.ActualQuantity;
                inventory.LastUpdated = DateTime.UtcNow;
                _branchInventoryRepository.Update(inventory);

                var product = await _productRepository.GetByIdAsync(item.ProductId);
                var productName = product?.ProductName ?? $"Unknown Product ({item.ProductId})";

                var title = $"Stock Adjustment - {productName}";
                if (title.Length > 255) title = title[..255];

                var auditMessage = $@"Type: StockAdjustment
Branch: {branchName} (ID: {branchId})
Product: {productName} (ID: {item.ProductId})
System Quantity: {oldQuantity}
Actual Quantity: {item.ActualQuantity}
Delta: {delta}
Reason: {item.Reason}
Adjusted By: {userName} (ID: {userId})
Adjusted At: {DateTime.UtcNow:O}";

                await _notificationService.CreateAuditLogAsync(userId, title, auditMessage);

                result.AdjustedItems.Add(new StockAdjustmentResultItemDto
                {
                    ProductId = item.ProductId,
                    ProductName = productName,
                    OldQuantity = oldQuantity,
                    ActualQuantity = item.ActualQuantity,
                    Delta = delta,
                    Reason = item.Reason,
                    AuditMessage = auditMessage
                });
            }

            await _unitOfWork.CommitTransactionAsync();
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }

        // In Phase 3, this is where we would trigger SignalR/FCM if delta is large

        return result;
    }
}
