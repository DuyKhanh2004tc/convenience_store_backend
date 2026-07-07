using System.ComponentModel.DataAnnotations;

namespace ConvenienceStore.Application.DTOs.User;

public class UpdateUserDto
{
    [Required]
    [StringLength(255)]
    public string FullName { get; set; } = string.Empty;

    [EmailAddress]
    [StringLength(100)]
    public string? Email { get; set; }

    [Phone]
    [StringLength(20)]
    public string? Phone { get; set; }

    [Required]
    public int RoleId { get; set; }

    public int? BranchId { get; set; }
    
    public bool IsActive { get; set; }
}
