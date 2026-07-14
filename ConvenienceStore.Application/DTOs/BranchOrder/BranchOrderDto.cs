namespace ConvenienceStore.Application.DTOs.BranchOrder;

public class BranchOrderDto
{
    public int OrderId { get; set; }
    public int BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public int CreatedBy { get; set; }
    public string CreatorName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<BranchOrderDetailDto> Items { get; set; } = new();
}

public class BranchOrderDetailDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
}
