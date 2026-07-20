using System.Collections.Concurrent;
using ConvenienceStore.Application.Contracts.Persistence;
using ConvenienceStore.Application.DTOs.GoodsReceipt;
using ConvenienceStore.Application.Services.Interfaces;
using ConvenienceStore.Domain.Entities;
using ConvenienceStore.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace ConvenienceStore.Application.Services;

public class GoodsReceiptService : IGoodsReceiptService
{
    private static readonly ConcurrentDictionary<int, SemaphoreSlim> PurchaseOrderLocks = new();
    private readonly IGenericRepository<GoodsReceipt> _receiptRepository;
    private readonly IGenericRepository<GoodsReceiptDetail> _receiptDetailRepository;
    private readonly IGenericRepository<PurchaseOrder> _poRepository;
    private readonly IGenericRepository<PurchaseOrderDetail> _poDetailRepository;
    private readonly IGenericRepository<WarehouseInventory> _warehouseInventoryRepository;
    private readonly IGenericRepository<Product> _productRepository;
    private readonly IGenericRepository<User> _userRepository;
    private readonly INotificationService _notificationService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GoodsReceiptService> _logger;

    public GoodsReceiptService(
        IGenericRepository<GoodsReceipt> receiptRepository,
        IGenericRepository<GoodsReceiptDetail> receiptDetailRepository,
        IGenericRepository<PurchaseOrder> poRepository,
        IGenericRepository<PurchaseOrderDetail> poDetailRepository,
        IGenericRepository<WarehouseInventory> warehouseInventoryRepository,
        IGenericRepository<Product> productRepository,
        IGenericRepository<User> userRepository,
        INotificationService notificationService,
        IUnitOfWork unitOfWork,
        ILogger<GoodsReceiptService> logger)
    {
        _receiptRepository = receiptRepository;
        _receiptDetailRepository = receiptDetailRepository;
        _poRepository = poRepository;
        _poDetailRepository = poDetailRepository;
        _warehouseInventoryRepository = warehouseInventoryRepository;
        _productRepository = productRepository;
        _userRepository = userRepository;
        _notificationService = notificationService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<GoodsReceiptDto> ReceiveGoodsAsync(CreateGoodsReceiptRequest request, int userId)
    {
        if (request.PoId <= 0)
            throw new Exception("Mã đơn đặt hàng không hợp lệ.");

        var purchaseOrderLock = PurchaseOrderLocks.GetOrAdd(
            request.PoId,
            _ => new SemaphoreSlim(1, 1));

        await purchaseOrderLock.WaitAsync();
        try
        {
            return await ReceiveGoodsCoreAsync(request, userId);
        }
        finally
        {
            purchaseOrderLock.Release();
        }
    }

    private async Task<GoodsReceiptDto> ReceiveGoodsCoreAsync(
        CreateGoodsReceiptRequest request,
        int userId)
    {
        var po = await _poRepository.GetByIdAsync(request.PoId);
        if (po == null) throw new Exception("Purchase Order not found.");

        if (po.Status != PurchaseOrderStatus.Pending)
            throw new Exception($"Cannot receive goods. PO status is {po.Status}");

        var existingReceipts = await _receiptRepository.GetAsync(r => r.PoId == request.PoId);
        if (existingReceipts.Any())
            throw new Exception("This Purchase Order has already been received.");

        if (request.Items == null || !request.Items.Any())
            throw new Exception("Receipt items cannot be empty.");

        var receiver = await _userRepository.GetByIdAsync(userId);
        if (receiver == null || !receiver.IsActive)
            throw new Exception("The goods receiver was not found or is inactive.");

        // Check duplicates
        var duplicates = request.Items.GroupBy(i => i.ProductId).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
        if (duplicates.Any())
            throw new Exception($"Duplicate product IDs found: {string.Join(", ", duplicates)}");

        var poDetails = await _poDetailRepository.GetAsync(d => d.PoId == request.PoId);
        if (!poDetails.Any())
            throw new Exception("Purchase Order has no items.");

        var missingProductIds = poDetails
            .Where(detail => request.Items.All(item => item.ProductId != detail.ProductId))
            .Select(detail => detail.ProductId)
            .ToList();
        if (missingProductIds.Any())
            throw new Exception($"Receipt must report the actual quantity for every ordered product. Missing product IDs: {string.Join(", ", missingProductIds)}");

        if (request.Items.All(item => item.QuantityReceived == 0))
            throw new Exception("At least one product must have a received quantity greater than 0.");

        // Validate the complete request before opening the transaction.
        var validatedItems = new List<(GoodsReceiptItemDto Item, Product Product)>();
        foreach (var item in request.Items)
        {
            if (item.QuantityReceived < 0)
                throw new Exception($"QuantityReceived must be >= 0 for product {item.ProductId}");

            var orderedItem = poDetails.FirstOrDefault(d => d.ProductId == item.ProductId);
            if (orderedItem == null)
                throw new Exception($"Product ID {item.ProductId} was not in the Purchase Order.");

            if (item.QuantityReceived > orderedItem.Quantity)
                throw new Exception($"Cannot receive more than ordered. Product ID {item.ProductId} ordered: {orderedItem.Quantity}, received: {item.QuantityReceived}");

            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product == null)
                throw new Exception($"Product ID {item.ProductId} not found.");
            if (product.ConversionFactor <= 0)
                throw new Exception($"Product '{product.ProductName}' has an invalid conversion factor.");

            validatedItems.Add((item, product));
        }

        await _unitOfWork.BeginTransactionAsync();

        GoodsReceipt receipt;
        var newlyLowStockProducts = new List<(int ProductId, string ProductName, int Quantity)>();
        try
        {
            receipt = new GoodsReceipt
            {
                PoId = po.PoId,
                ReceivedBy = userId,
                ReceivedDate = DateTime.UtcNow,
                Notes = request.Notes
            };

            await _receiptRepository.AddAsync(receipt);
            await _unitOfWork.SaveChangesAsync(); // save to get ReceiptId

            var receiptDetails = new List<GoodsReceiptDetail>();

            foreach (var (item, product) in validatedItems)
            {
                receiptDetails.Add(new GoodsReceiptDetail
                {
                    ReceiptId = receipt.ReceiptId,
                    ProductId = item.ProductId,
                    QuantityReceived = item.QuantityReceived
                });

                var stockToAdd = item.QuantityReceived * product.ConversionFactor;

                if (stockToAdd == 0)
                    continue;

                var warehouseInventory = (await _warehouseInventoryRepository.GetAsync(wi => wi.ProductId == item.ProductId)).FirstOrDefault();

                if (warehouseInventory == null)
                {
                    await _warehouseInventoryRepository.AddAsync(new WarehouseInventory
                    {
                        ProductId = item.ProductId,
                        Quantity = stockToAdd,
                        LastUpdated = DateTime.UtcNow
                    });

                    if (stockToAdd <= 50)
                    {
                        newlyLowStockProducts.Add((
                            item.ProductId,
                            product.ProductName,
                            stockToAdd));
                    }
                }
                else
                {
                    warehouseInventory.Quantity += stockToAdd;
                    warehouseInventory.LastUpdated = DateTime.UtcNow;
                    _warehouseInventoryRepository.Update(warehouseInventory);
                }
            }

            await _receiptDetailRepository.AddRangeAsync(receiptDetails);

            po.Status = PurchaseOrderStatus.Received;
            po.UpdatedAt = DateTime.UtcNow;
            _poRepository.Update(po);

            await _unitOfWork.CommitTransactionAsync();
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }

        var result = await BuildGoodsReceiptDto(receipt);
        var purchaseOrderCode = $"PO-{po.OrderDate:yyyyMMdd}-{po.PoId:D6}";

        foreach (var product in newlyLowStockProducts)
        {
            try
            {
                await _notificationService.CreateAlertForRolesAsync(
                    new List<string> { "Chain Director", "Warehouse Staff" },
                    $"Cảnh báo sắp hết hàng - {product.ProductName}",
                    $"Sản phẩm {product.ProductName} (ID: {product.ProductId}) vừa được nhập vào kho tổng " +
                    $"với số lượng tồn {product.Quantity}. Mức tồn hiện tại đang bằng hoặc dưới " +
                    "ngưỡng cảnh báo 50. Vui lòng kiểm tra và lập kế hoạch nhập thêm hàng.");
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Could not send low-stock notification for newly received product {ProductId}.",
                    product.ProductId);
            }
        }

        // Receiving inventory has already committed; notification failures must
        // not make the client retry and count the same receipt twice.
        try
        {
            await _notificationService.CreateAuditLogAsync(
                userId,
                $"Đã nhận hàng cho {purchaseOrderCode}",
                $"Đã hoàn tất nhận hàng cho {purchaseOrderCode}. Số mặt hàng: {request.Items.Count}.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not create the goods-receipt notification for PO {PoId}.", po.PoId);
        }

        var notificationTitle = result.HasShortage
            ? $"Nhà cung cấp giao thiếu - {purchaseOrderCode}"
            : $"Đã nhận đủ hàng - {purchaseOrderCode}";
        string directorMessage;

        if (result.HasShortage)
        {
            var shortageLines = result.Items
                .Where(item => item.ShortageQuantity > 0)
                .Select(item =>
                    $"- {item.ProductName}: đặt {item.OrderedQuantity} {item.ImportUnit ?? "đơn vị nhập"}, " +
                    $"thực nhận {item.QuantityReceived}, thiếu {item.ShortageQuantity}");

            directorMessage =
                $"{purchaseOrderCode} đã được đóng sau khi ghi nhận nhà cung cấp giao thiếu.\n" +
                string.Join("\n", shortageLines) +
                "\nVui lòng làm việc lại với nhà cung cấp. Bạn có thể mở thông báo này để xem chi tiết và tạo PO bổ sung nếu cần.";
        }
        else
        {
            directorMessage =
                $"{purchaseOrderCode} đã được nhận đủ và đóng thành công. Không cần thực hiện thêm thao tác nào.";
        }

        try
        {
            await _notificationService.CreateAlertForRolesAsync(
                new List<string> { "Chain Director" },
                notificationTitle,
                directorMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not notify Chain Directors about receipt result for PO {PoId}.", po.PoId);
        }

        return result;
    }

    public async Task<IEnumerable<GoodsReceiptDto>> GetAllGoodsReceiptsAsync()
    {
        var receipts = await _receiptRepository.GetAllAsync();
        var result = new List<GoodsReceiptDto>();

        foreach (var receipt in receipts.OrderByDescending(r => r.ReceivedDate))
        {
            result.Add(await BuildGoodsReceiptDto(receipt));
        }

        return result;
    }

    public async Task<GoodsReceiptDto?> GetGoodsReceiptByIdAsync(int id)
    {
        var receipt = await _receiptRepository.GetByIdAsync(id);
        if (receipt == null) return null;

        return await BuildGoodsReceiptDto(receipt);
    }

    private async Task<GoodsReceiptDto> BuildGoodsReceiptDto(GoodsReceipt receipt)
    {
        var receiver = await _userRepository.GetByIdAsync(receipt.ReceivedBy);
        var details = await _receiptDetailRepository.GetAsync(d => d.ReceiptId == receipt.ReceiptId);
        var poDetails = await _poDetailRepository.GetAsync(d => d.PoId == receipt.PoId);

        var itemDtos = new List<GoodsReceiptItemDto>();
        foreach (var d in details)
        {
            var product = await _productRepository.GetByIdAsync(d.ProductId);
            var orderedQuantity = poDetails.FirstOrDefault(item => item.ProductId == d.ProductId)?.Quantity ?? 0;
            itemDtos.Add(new GoodsReceiptItemDto
            {
                ProductId = d.ProductId,
                ProductName = product?.ProductName ?? "",
                OrderedQuantity = orderedQuantity,
                QuantityReceived = d.QuantityReceived,
                Unit = product?.Unit,
                ImportUnit = product?.ImportUnit,
                ConversionFactor = product?.ConversionFactor ?? 1
            });
        }

        return new GoodsReceiptDto
        {
            ReceiptId = receipt.ReceiptId,
            PoId = receipt.PoId,
            ReceivedBy = receipt.ReceivedBy,
            ReceiverName = receiver?.FullName ?? "",
            ReceivedDate = receipt.ReceivedDate,
            Notes = receipt.Notes,
            Items = itemDtos
        };
    }
}
