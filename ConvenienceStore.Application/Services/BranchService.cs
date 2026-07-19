using ConvenienceStore.Application.Contracts.Persistence;
using ConvenienceStore.Application.DTOs.Branch;
using ConvenienceStore.Application.Services.Interfaces;
using ConvenienceStore.Domain.Entities;

namespace ConvenienceStore.Application.Services;

public class BranchService : IBranchService
{
    private readonly IGenericRepository<Branch> _branchRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BranchService(
        IGenericRepository<Branch> branchRepository,
        IUnitOfWork unitOfWork)
    {
        _branchRepository = branchRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<BranchDto>> GetAllBranchesAsync()
    {
        var branches = await _branchRepository.GetAllAsync();
        return branches.Select(b => new BranchDto
        {
            BranchId = b.BranchId,
            BranchName = b.BranchName,
            Address = b.Address,
            Phone = b.Phone,
            IsActive = b.IsActive
        }).ToList();
    }

    public async Task<BranchDto?> GetBranchByIdAsync(int id)
    {
        var b = await _branchRepository.GetByIdAsync(id);
        if (b == null) return null;

        return new BranchDto
        {
            BranchId = b.BranchId,
            BranchName = b.BranchName,
            Address = b.Address,
            Phone = b.Phone,
            IsActive = b.IsActive
        };
    }

    public async Task<BranchDto> CreateBranchAsync(CreateBranchDto dto)
    {
        var branch = new Branch
        {
            BranchName = dto.BranchName,
            Address = dto.Address,
            Phone = dto.Phone,
            IsActive = dto.IsActive
        };

        var created = await _branchRepository.AddAsync(branch);
        await _unitOfWork.SaveChangesAsync();

        return new BranchDto
        {
            BranchId = created.BranchId,
            BranchName = created.BranchName,
            Address = created.Address,
            Phone = created.Phone,
            IsActive = created.IsActive
        };
    }

    public async Task<bool> UpdateBranchAsync(int id, CreateBranchDto dto)
    {
        var branch = await _branchRepository.GetByIdAsync(id);
        if (branch == null) return false;

        branch.BranchName = dto.BranchName;
        branch.Address = dto.Address;
        branch.Phone = dto.Phone;
        branch.IsActive = dto.IsActive;

        _branchRepository.Update(branch);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteBranchAsync(int id)
    {
        var branch = await _branchRepository.GetByIdAsync(id, "Users", "BranchInventories", "BranchOrders", "Invoices", "Shifts");
        if (branch == null) return false;

        if (branch.Users.Any() || 
            branch.BranchInventories.Any() || 
            branch.BranchOrders.Any() || 
            branch.Invoices.Any() || 
            branch.Shifts.Any())
        {
            throw new Exception("Không thể xóa chi nhánh vì đã có dữ liệu ràng buộc (Nhân sự, Tồn kho, Đơn hàng, Hóa đơn...). Vui lòng chỉ vô hiệu hóa (Ngừng hoạt động) chi nhánh này.");
        }

        _branchRepository.Delete(branch);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
