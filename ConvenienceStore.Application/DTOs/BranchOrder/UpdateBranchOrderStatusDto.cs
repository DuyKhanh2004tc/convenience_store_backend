using System.ComponentModel.DataAnnotations;

namespace ConvenienceStore.Application.DTOs.BranchOrder;

public class UpdateBranchOrderStatusDto
{
    /// <summary>
    /// Accepted values: "Processing", "Fulfilled", "Completed", "Cancelled"
    /// </summary>
    [Required]
    public string Status { get; set; } = string.Empty;
}
