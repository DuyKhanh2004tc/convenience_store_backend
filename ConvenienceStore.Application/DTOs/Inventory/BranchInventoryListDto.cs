namespace ConvenienceStore.Application.DTOs.Inventory;

public class BranchInventoryListDto
{
    public IEnumerable<BranchInventoryDto> Items { get; set; } = new List<BranchInventoryDto>();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
}
