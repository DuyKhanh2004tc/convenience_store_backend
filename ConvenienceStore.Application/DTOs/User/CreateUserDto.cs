using System.ComponentModel.DataAnnotations;

namespace ConvenienceStore.Application.DTOs.User;

public class CreateUserDto
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", 
        ErrorMessage = "Password must contain at least 8 characters, including one uppercase letter, one number, and one special character.")]
    public string Password { get; set; } = string.Empty;

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
}
