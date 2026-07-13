namespace ConvenienceStore.Application.DTOs.Branch;

public class BranchDto
{
    public int BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public bool IsActive { get; set; }
}

public class CreateBranchDto
{
    public string BranchName { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public bool IsActive { get; set; } = true;
}
