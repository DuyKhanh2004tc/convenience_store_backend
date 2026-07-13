using ConvenienceStore.Application.DTOs.Branch;

namespace ConvenienceStore.Application.Services.Interfaces;

public interface IBranchService
{
    Task<IEnumerable<BranchDto>> GetAllBranchesAsync();
    Task<BranchDto?> GetBranchByIdAsync(int id);
    Task<BranchDto> CreateBranchAsync(CreateBranchDto dto);
    Task<bool> UpdateBranchAsync(int id, CreateBranchDto dto);
    Task<bool> DeleteBranchAsync(int id);
}
