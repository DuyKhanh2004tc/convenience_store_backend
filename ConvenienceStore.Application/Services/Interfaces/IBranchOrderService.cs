using ConvenienceStore.Application.DTOs.BranchOrder;

namespace ConvenienceStore.Application.Services.Interfaces;

public interface IBranchOrderService
{
    /// <summary>
    /// Store Manager creates a new internal order from their branch to the warehouse.
    /// </summary>
    Task<BranchOrderDto> CreateOrderAsync(int createdByUserId, CreateBranchOrderDto request);

    /// <summary>
    /// Get all orders, optionally filtered by branch or status.
    /// </summary>
    Task<IEnumerable<BranchOrderDto>> GetAllOrdersAsync(int? branchId = null, string? status = null);

    /// <summary>
    /// Get a single order by its ID with full details.
    /// </summary>
    Task<BranchOrderDto?> GetOrderByIdAsync(int orderId);

    /// <summary>
    /// Get all orders belonging to a specific branch.
    /// </summary>
    Task<IEnumerable<BranchOrderDto>> GetOrdersByBranchAsync(int branchId);

    /// <summary>
    /// Warehouse Staff approves an order: deducts from WarehouseInventory, adds to BranchInventory.
    /// Uses database transaction for data consistency.
    /// </summary>
    Task<BranchOrderDto> ApproveOrderAsync(int orderId);

    /// <summary>
    /// Warehouse Staff or Store Manager cancels a pending order.
    /// </summary>
    Task<BranchOrderDto> CancelOrderAsync(int orderId);
}
