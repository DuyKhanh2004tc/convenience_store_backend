using ConvenienceStore.Application.Contracts.Persistence;
using ConvenienceStore.Application.DTOs.Employee;
using ConvenienceStore.Application.Services.Interfaces;
using ConvenienceStore.Domain.Entities;

namespace ConvenienceStore.Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IGenericRepository<User> _userRepository;
    private readonly IGenericRepository<Branch> _branchRepository;
    private readonly IGenericRepository<Role> _roleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EmployeeService(
        IGenericRepository<User> userRepository,
        IGenericRepository<Branch> branchRepository,
        IGenericRepository<Role> roleRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _branchRepository = branchRepository;
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto dto)
    {
        // Check for duplicate username
        var existingUsers = await _userRepository.GetAsync(u => u.Username == dto.Username);
        if (existingUsers.Any())
            throw new InvalidOperationException($"Username '{dto.Username}' already exists.");

        var user = new User
        {
            Username = dto.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            FullName = dto.FullName,
            Email = dto.Email,
            Phone = dto.Phone,
            Position = dto.Position,
            BranchId = dto.BranchId,
            RoleId = dto.RoleId,
            JoinDate = DateTime.UtcNow,
            IsActive = true
        };

        var created = await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        // Reload related entities for the response
        var role = await _roleRepository.GetByIdAsync(created.RoleId);
        Branch? branch = created.BranchId.HasValue
            ? await _branchRepository.GetByIdAsync(created.BranchId.Value)
            : null;

        return MapToDto(created, role?.RoleName, branch?.BranchName);
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
    {
        var users = await _userRepository.GetAllAsync();
        var roles = await _roleRepository.GetAllAsync();
        var branches = await _branchRepository.GetAllAsync();

        return users.Select(u => MapToDto(
            u,
            roles.FirstOrDefault(r => r.RoleId == u.RoleId)?.RoleName,
            branches.FirstOrDefault(b => b.BranchId == u.BranchId)?.BranchName
        )).ToList();
    }

    public async Task<IEnumerable<EmployeeDto>> GetEmployeesByBranchAsync(int branchId)
    {
        var users = await _userRepository.GetAsync(u => u.BranchId == branchId);
        var roles = await _roleRepository.GetAllAsync();
        var branch = await _branchRepository.GetByIdAsync(branchId);

        return users.Select(u => MapToDto(
            u,
            roles.FirstOrDefault(r => r.RoleId == u.RoleId)?.RoleName,
            branch?.BranchName
        )).ToList();
    }

    public async Task<EmployeeDto?> GetEmployeeByIdAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return null;

        var role = await _roleRepository.GetByIdAsync(user.RoleId);
        Branch? branch = user.BranchId.HasValue
            ? await _branchRepository.GetByIdAsync(user.BranchId.Value)
            : null;

        return MapToDto(user, role?.RoleName, branch?.BranchName);
    }

    public async Task<bool> UpdateEmployeeAsync(int userId, UpdateEmployeeDto dto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return false;

        user.FullName = dto.FullName;
        user.Email = dto.Email;
        user.Phone = dto.Phone;
        user.Position = dto.Position;
        user.BranchId = dto.BranchId;
        user.IsActive = dto.IsActive;
        user.UpdatedAt = DateTime.UtcNow;

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ToggleEmployeeStatusAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return false;

        user.IsActive = !user.IsActive;
        user.UpdatedAt = DateTime.UtcNow;

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    private static EmployeeDto MapToDto(User user, string? roleName, string? branchName)
    {
        return new EmployeeDto
        {
            UserId = user.UserId,
            Username = user.Username,
            FullName = user.FullName,
            Email = user.Email,
            Phone = user.Phone,
            Position = user.Position,
            JoinDate = user.JoinDate,
            IsActive = user.IsActive,
            BranchId = user.BranchId,
            BranchName = branchName,
            RoleId = user.RoleId,
            RoleName = roleName
        };
    }
}
