using System.ComponentModel.DataAnnotations;

namespace ConvenienceStore.Application.DTOs.Customer;

public class CreateCustomerDto
{
    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [Phone]
    public string Phone { get; set; } = string.Empty;

    public string? Email { get; set; }
}
