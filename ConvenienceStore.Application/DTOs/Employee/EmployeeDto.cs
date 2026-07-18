namespace ConvenienceStore.Application.DTOs.Employee;

public class EmployeeDto
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Position { get; set; }
    public DateTime? JoinDate { get; set; }
    public bool IsActive { get; set; }
    public int? BranchId { get; set; }
    public string? BranchName { get; set; }
    public int RoleId { get; set; }
    public string? RoleName { get; set; }
}

public class CreateEmployeeDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Position { get; set; }
    public int? BranchId { get; set; }
    public int RoleId { get; set; }
}

public class UpdateEmployeeDto
{
    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Position { get; set; }
    public int? BranchId { get; set; }
    public bool IsActive { get; set; }
}
