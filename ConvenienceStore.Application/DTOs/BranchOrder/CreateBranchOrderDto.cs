using System.ComponentModel.DataAnnotations;

namespace ConvenienceStore.Application.DTOs.BranchOrder;

public class CreateBranchOrderDto
{
    [Required]
    public int BranchId { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "Order must contain at least one item.")]
    public List<CreateBranchOrderItemDto> Items { get; set; } = new();
}

public class CreateBranchOrderItemDto
{
    [Required]
    public int ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; }
}
