using System.Collections.Concurrent;
using ConvenienceStore.Application.Constants;
using ConvenienceStore.Application.Contracts.Persistence;
using ConvenienceStore.Application.DTOs.Inventory;
using ConvenienceStore.Application.Services.Interfaces;
using ConvenienceStore.Domain.Entities;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace ConvenienceStore.Application.Services;

public class BranchInventoryService : IBranchInventoryService
{
    private static readonly ConcurrentDictionary<string, SemaphoreSlim> InventoryLocks = new();
    private readonly IGenericRepository<BranchInventory> _branchInventoryRepository;
    private readonly IGenericRepository<Product> _productRepository;
    private readonly IGenericRepository<Branch> _branchRepository;
    private readonly INotificationService _notificationService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<BranchInventoryService> _logger;

    public BranchInventoryService(
        IGenericRepository<BranchInventory> branchInventoryRepository,
        IGenericRepository<Product> productRepository,
        IGenericRepository<Branch> branchRepository,
        INotificationService notificationService,
        IUnitOfWork unitOfWork,
        ILogger<BranchInventoryService> logger)
    {
        _branchInventoryRepository = branchInventoryRepository;
        _productRepository = productRepository;
        _branchRepository = branchRepository;
        _notificationService = notificationService;
        _unitOfWork = unitOfWork;
        _logger = logger;
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

        var branch = await _branchRepository.GetByIdAsync(branchId);
        if (branch == null || !branch.IsActive)
            throw new Exception("Chi nhánh không tồn tại hoặc đã ngừng hoạt động.");

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
        
        var branchName = branch.BranchName;

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
                Unit = x.p.Unit,
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
            Unit = product.Unit,
            Quantity = inventory.Quantity,
            LastUpdated = inventory.LastUpdated
        };
    }

    public async Task<IEnumerable<BranchInventoryDto>> GetLowStockByBranchAsync(int branchId, int threshold, ClaimsPrincipal currentUser)
    {
        ValidateBranchAccess(branchId, currentUser);

        if (threshold < 0)
            throw new Exception("Ngưỡng tồn kho thấp không được là số âm.");

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
                Unit = p.Unit,
                Quantity = bi.Quantity,
                LastUpdated = bi.LastUpdated
            }).ToList();

        return joined;
    }

    public async Task<IEnumerable<BranchInventoryDto>> GetAllLowStockAsync(int threshold)
    {
        var query = await _branchInventoryRepository.GetAsync(bi => bi.Quantity <= threshold);
        var allProducts = await _productRepository.GetAllAsync();
        var allBranches = await _branchRepository.GetAllAsync();

        var joined = query.Join(allProducts, 
            bi => bi.ProductId, 
            p => p.ProductId, 
            (bi, p) => new { bi, p })
            .Join(allBranches,
            x => x.bi.BranchId,
            b => b.BranchId,
            (x, b) => new BranchInventoryDto
            {
                BranchId = b.BranchId,
                BranchName = b.BranchName,
                ProductId = x.bi.ProductId,
                ProductName = x.p.ProductName,
                Barcode = x.p.Barcode,
                Quantity = x.bi.Quantity,
                LastUpdated = x.bi.LastUpdated
            }).ToList();

        return joined;
    }

    public async Task<StockAdjustmentResultDto> AdjustStockAsync(int branchId, StockAdjustmentRequest request, ClaimsPrincipal currentUser)
    {
        var productIds = request.Items?
            .Select(item => item.ProductId)
            .Distinct()
            .OrderBy(id => id)
            .ToList() ?? new List<int>();
        var acquiredLocks = new List<SemaphoreSlim>();

        try
        {
            foreach (var productId in productIds)
            {
                var inventoryLock = InventoryLocks.GetOrAdd(
                    $"{branchId}:{productId}",
                    _ => new SemaphoreSlim(1, 1));
                await inventoryLock.WaitAsync();
                acquiredLocks.Add(inventoryLock);
            }

            return await AdjustStockCoreAsync(branchId, request, currentUser);
        }
        finally
        {
            for (var index = acquiredLocks.Count - 1; index >= 0; index--)
                acquiredLocks[index].Release();
        }
    }

    private async Task<StockAdjustmentResultDto> AdjustStockCoreAsync(
        int branchId,
        StockAdjustmentRequest request,
        ClaimsPrincipal currentUser)
    {
        ValidateBranchAccess(branchId, currentUser);

        var role = currentUser.FindFirstValue(ClaimTypes.Role);
        if (role != "Store Manager")
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
        var branchName = branch?.BranchName ?? $"Chi nhánh không xác định ({branchId})";

        // Validate branch is active
        if (branch == null || !branch.IsActive)
            throw new Exception("Chi nhánh đã ngừng hoạt động. Không thể điều chỉnh tồn kho.");

        var result = new StockAdjustmentResultDto
        {
            BranchId = branchId,
            BranchName = branchName
        };
        var auditLogs = new List<(string Title, string Message)>();

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            foreach (var item in request.Items)
            {
                if (item.ActualQuantity < 0)
                    throw new Exception($"ActualQuantity cannot be negative for product {item.ProductId}");

                if (!AdjustmentReasons.IsValid(item.Reason))
                    throw new Exception($"Invalid reason '{item.Reason}' for product {item.ProductId}");

                var note = item.Note?.Trim();
                if (note?.Length > 500)
                    throw new Exception(
                        $"Ghi chú của sản phẩm {item.ProductId} không được vượt quá 500 ký tự.");

                var inventory = (await _branchInventoryRepository.GetAsync(bi => bi.BranchId == branchId && bi.ProductId == item.ProductId)).FirstOrDefault();
                if (inventory == null)
                    throw new Exception($"Inventory record not found for product {item.ProductId} in branch {branchId}");

                if (inventory.Quantity != item.ExpectedQuantity)
                    throw new InvalidOperationException(
                        $"Tồn kho sản phẩm {item.ProductId} đã thay đổi từ {item.ExpectedQuantity} thành {inventory.Quantity}. " +
                        "Vui lòng tải lại dữ liệu và kiểm kê lại sản phẩm này.");

                var oldQuantity = inventory.Quantity;
                var delta = item.ActualQuantity - oldQuantity;

                if (delta == 0) continue; // No change

                if (item.Reason == AdjustmentReasons.Other && string.IsNullOrWhiteSpace(note))
                    throw new Exception(
                        $"Vui lòng nhập ghi chú khi chọn lý do 'Khác' cho sản phẩm {item.ProductId}.");

                var validReasonForDelta = delta < 0
                    ? item.Reason is AdjustmentReasons.Damaged or AdjustmentReasons.Expired or AdjustmentReasons.Lost or AdjustmentReasons.Other
                    : item.Reason is AdjustmentReasons.Found or AdjustmentReasons.Other;
                if (!validReasonForDelta)
                    throw new Exception(
                        $"Lý do '{item.Reason}' không phù hợp với chênh lệch {delta} của sản phẩm {item.ProductId}.");

                inventory.Quantity = item.ActualQuantity;
                inventory.LastUpdated = DateTime.UtcNow;
                _branchInventoryRepository.Update(inventory);

                var product = await _productRepository.GetByIdAsync(item.ProductId);
                var productName = product?.ProductName ?? $"Sản phẩm không xác định ({item.ProductId})";

                // Validate: cannot increase stock for deactivated products
                if (product != null && !product.IsActive && delta > 0)
                    throw new Exception($"Không thể tăng tồn kho cho sản phẩm '{productName}' đã ngừng kinh doanh.");

                var reasonLabel = item.Reason switch
                {
                    AdjustmentReasons.Damaged => "Hàng hư hỏng",
                    AdjustmentReasons.Expired => "Hàng hết hạn",
                    AdjustmentReasons.Lost => "Mất mát hàng hóa",
                    AdjustmentReasons.Found => "Phát hiện hàng thừa",
                    _ => "Chênh lệch khác"
                };
                var title = $"Kiểm kê: {reasonLabel} - {productName}";
                if (title.Length > 255) title = title[..255];

                var auditMessage = $@"Loại nghiệp vụ: Kiểm kê và điều chỉnh tồn kho
Chi nhánh: {branchName} (ID: {branchId})
Sản phẩm: {productName} (ID: {item.ProductId})
Số lượng trên hệ thống: {oldQuantity}
Số lượng thực tế: {item.ActualQuantity}
Chênh lệch: {delta} {product?.Unit ?? "đơn vị"}
Tình trạng: {reasonLabel}
Ghi chú: {note ?? "Không có"}
Người điều chỉnh: {userName} (ID: {userId})
Thời gian điều chỉnh: {DateTime.UtcNow:O}";

                auditLogs.Add((title, auditMessage));

                result.AdjustedItems.Add(new StockAdjustmentResultItemDto
                {
                    ProductId = item.ProductId,
                    ProductName = productName,
                    OldQuantity = oldQuantity,
                    ActualQuantity = item.ActualQuantity,
                    Delta = delta,
                    Reason = item.Reason,
                    Note = note,
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

        // Broadcast only after the inventory transaction commits. A notification
        // failure must not make the client repeat a completed stock adjustment.
        foreach (var auditLog in auditLogs)
        {
            try
            {
                await _notificationService.CreateAuditLogAsync(
                    userId,
                    auditLog.Title,
                    auditLog.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not create stock-adjustment audit notification for branch {BranchId}.", branchId);
            }
        }

        return result;
    }
}
