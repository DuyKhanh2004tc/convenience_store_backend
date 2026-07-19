using ConvenienceStore.Application.Contracts.Persistence;
using ConvenienceStore.Application.DTOs.GoodsReceipt;
using ConvenienceStore.Application.Services.Interfaces;
using ConvenienceStore.Domain.Entities;
using ConvenienceStore.Domain.Enums;

namespace ConvenienceStore.Application.Services;

public class GoodsReceiptService : IGoodsReceiptService
{
    private readonly IGenericRepository<GoodsReceipt> _receiptRepository;
    private readonly IGenericRepository<GoodsReceiptDetail> _receiptDetailRepository;
    private readonly IGenericRepository<PurchaseOrder> _poRepository;
    private readonly IGenericRepository<PurchaseOrderDetail> _poDetailRepository;
    private readonly IGenericRepository<WarehouseInventory> _warehouseInventoryRepository;
    private readonly IGenericRepository<Product> _productRepository;
    private readonly IGenericRepository<User> _userRepository;
    private readonly INotificationService _notificationService;
    private readonly IUnitOfWork _unitOfWork;

    public GoodsReceiptService(
        IGenericRepository<GoodsReceipt> receiptRepository,
        IGenericRepository<GoodsReceiptDetail> receiptDetailRepository,
        IGenericRepository<PurchaseOrder> poRepository,
        IGenericRepository<PurchaseOrderDetail> poDetailRepository,
        IGenericRepository<WarehouseInventory> warehouseInventoryRepository,
        IGenericRepository<Product> productRepository,
        IGenericRepository<User> userRepository,
        INotificationService notificationService,
        IUnitOfWork unitOfWork)
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
    }

    public async Task<GoodsReceiptDto> ReceiveGoodsAsync(CreateGoodsReceiptRequest request, int userId)
    {
        var po = await _poRepository.GetByIdAsync(request.PoId);
        if (po == null) throw new Exception("Purchase Order not found.");

        if (po.Status != PurchaseOrderStatus.Pending)
            throw new Exception($"Cannot receive goods. PO status is {po.Status}");

        if (request.Items == null || !request.Items.Any())
            throw new Exception("Receipt items cannot be empty.");

        // Check duplicates
        var duplicates = request.Items.GroupBy(i => i.ProductId).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
        if (duplicates.Any())
            throw new Exception($"Duplicate product IDs found: {string.Join(", ", duplicates)}");

        var poDetails = await _poDetailRepository.GetAsync(d => d.PoId == request.PoId);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var receipt = new GoodsReceipt
            {
                PoId = po.PoId,
                ReceivedBy = userId,
                ReceivedDate = DateTime.UtcNow,
                Notes = request.Notes
            };

            await _receiptRepository.AddAsync(receipt);
            await _unitOfWork.SaveChangesAsync(); // save to get ReceiptId

            var receiptDetails = new List<GoodsReceiptDetail>();

            foreach (var item in request.Items)
            {
                if (item.QuantityReceived <= 0)
                    throw new Exception($"QuantityReceived must be > 0 for product {item.ProductId}");

                var orderedItem = poDetails.FirstOrDefault(d => d.ProductId == item.ProductId);
                if (orderedItem == null)
                    throw new Exception($"Product ID {item.ProductId} was not in the Purchase Order.");

                if (item.QuantityReceived > orderedItem.Quantity)
                    throw new Exception($"Cannot receive more than ordered. Product ID {item.ProductId} ordered: {orderedItem.Quantity}, received: {item.QuantityReceived}");

                receiptDetails.Add(new GoodsReceiptDetail
                {
                    ReceiptId = receipt.ReceiptId,
                    ProductId = item.ProductId,
                    QuantityReceived = item.QuantityReceived
                });

                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null) throw new Exception($"Product ID {item.ProductId} not found.");
                if (product.ConversionFactor <= 0) product.ConversionFactor = 1;

                var stockToAdd = item.QuantityReceived * product.ConversionFactor;

                var warehouseInventory = (await _warehouseInventoryRepository.GetAsync(wi => wi.ProductId == item.ProductId)).FirstOrDefault();

                if (warehouseInventory == null)
                {
                    await _warehouseInventoryRepository.AddAsync(new WarehouseInventory
                    {
                        ProductId = item.ProductId,
                        Quantity = stockToAdd,
                        LastUpdated = DateTime.UtcNow
                    });
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
            
            // Log notification after successful commit
            await _notificationService.CreateAuditLogAsync(userId, $"PO #{po.PoId} Received", $"Received goods for PO #{po.PoId}. Items: {request.Items.Count}");

            return await BuildGoodsReceiptDto(receipt);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
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

        var itemDtos = new List<GoodsReceiptItemDto>();
        foreach (var d in details)
        {
            var product = await _productRepository.GetByIdAsync(d.ProductId);
            itemDtos.Add(new GoodsReceiptItemDto
            {
                ProductId = d.ProductId,
                ProductName = product?.ProductName ?? "",
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
