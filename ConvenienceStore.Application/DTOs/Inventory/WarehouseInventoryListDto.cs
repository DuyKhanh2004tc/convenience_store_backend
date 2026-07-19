namespace ConvenienceStore.Application.DTOs.Inventory;

public class WarehouseInventoryListDto
{
    public IEnumerable<WarehouseInventoryDto> Items { get; set; } = new List<WarehouseInventoryDto>();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
}
