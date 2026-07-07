using ConvenienceStore.Application.Contracts.Persistence;
using ConvenienceStore.Application.DTOs.User;
using ConvenienceStore.Application.Services.Interfaces;
using ConvenienceStore.Domain.Entities;

namespace ConvenienceStore.Application.Services;

public class UserService : IUserService
{
    private readonly IGenericRepository<User> _userRepository;
    private readonly IGenericRepository<Role> _roleRepository;
    private readonly IGenericRepository<Branch> _branchRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(
        IGenericRepository<User> userRepository,
        IGenericRepository<Role> roleRepository,
        IGenericRepository<Branch> branchRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _branchRepository = branchRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        var roles = await _roleRepository.GetAllAsync();
        var branches = await _branchRepository.GetAllAsync();

        return users.Select(u => new UserDto
        {
            UserId = u.UserId,
            Username = u.Username,
            FullName = u.FullName,
            Email = u.Email,
            Phone = u.Phone,
            RoleId = u.RoleId,
            RoleName = roles.FirstOrDefault(r => r.RoleId == u.RoleId)?.RoleName ?? "",
            BranchId = u.BranchId,
            BranchName = branches.FirstOrDefault(b => b.BranchId == u.BranchId)?.BranchName,
            IsActive = u.IsActive,
            CreatedAt = u.CreatedAt
        });
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return null;

        var role = await _roleRepository.GetByIdAsync(user.RoleId);
        var branch = user.BranchId.HasValue ? await _branchRepository.GetByIdAsync(user.BranchId.Value) : null;

        return new UserDto
        {
            UserId = user.UserId,
            Username = user.Username,
            FullName = user.FullName,
            Email = user.Email,
            Phone = user.Phone,
            RoleId = user.RoleId,
            RoleName = role?.RoleName ?? "",
            BranchId = user.BranchId,
            BranchName = branch?.BranchName,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt
        };
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto request)
    {
        // 1. Validate Username uniqueness
        var existingUsername = await _userRepository.GetAsync(u => u.Username == request.Username);
        if (existingUsername.Any())
            throw new Exception("Username already exists."); // In production, throw custom exceptions to return specific status codes

        // 2. Validate Phone uniqueness (BR-13)
        if (!string.IsNullOrEmpty(request.Phone))
        {
            var existingPhone = await _userRepository.GetAsync(u => u.Phone == request.Phone);
            if (existingPhone.Any())
                throw new Exception("MSG-16: Phone number already exists.");
        }

        // 3. Validate Email uniqueness
        if (!string.IsNullOrEmpty(request.Email))
        {
            var existingEmail = await _userRepository.GetAsync(u => u.Email == request.Email);
            if (existingEmail.Any())
                throw new Exception("MSG-15: Email already exists.");
        }

        // 4. Validate Role
        var role = await _roleRepository.GetByIdAsync(request.RoleId);
        if (role == null)
            throw new Exception("Role does not exist.");

        // 5. Validate Branch if provided
        Branch? branch = null;
        if (request.BranchId.HasValue)
        {
            branch = await _branchRepository.GetByIdAsync(request.BranchId.Value);
            if (branch == null)
                throw new Exception("Branch does not exist.");
        }

        var newUser = new User
        {
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            FullName = request.FullName,
            Email = request.Email,
            Phone = request.Phone,
            RoleId = request.RoleId,
            BranchId = request.BranchId,
            IsActive = true
        };

        await _userRepository.AddAsync(newUser);
        await _unitOfWork.SaveChangesAsync();

        return new UserDto
        {
            UserId = newUser.UserId,
            Username = newUser.Username,
            FullName = newUser.FullName,
            Email = newUser.Email,
            Phone = newUser.Phone,
            RoleId = newUser.RoleId,
            RoleName = role.RoleName,
            BranchId = newUser.BranchId,
            BranchName = branch?.BranchName,
            IsActive = newUser.IsActive,
            CreatedAt = newUser.CreatedAt
        };
    }

    public async Task<UserDto> UpdateUserAsync(int id, UpdateUserDto request)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new Exception("User not found.");

        // Validate Phone uniqueness (BR-13)
        if (!string.IsNullOrEmpty(request.Phone) && request.Phone != user.Phone)
        {
            var existingPhone = await _userRepository.GetAsync(u => u.Phone == request.Phone && u.UserId != id);
            if (existingPhone.Any())
                throw new Exception("MSG-16: Phone number already exists.");
        }

        // Validate Email uniqueness
        if (!string.IsNullOrEmpty(request.Email) && request.Email != user.Email)
        {
            var existingEmail = await _userRepository.GetAsync(u => u.Email == request.Email && u.UserId != id);
            if (existingEmail.Any())
                throw new Exception("MSG-15: Email already exists.");
        }

        // Validate Role
        var role = await _roleRepository.GetByIdAsync(request.RoleId);
        if (role == null)
            throw new Exception("Role does not exist.");

        // Validate Branch if provided
        Branch? branch = null;
        if (request.BranchId.HasValue)
        {
            branch = await _branchRepository.GetByIdAsync(request.BranchId.Value);
            if (branch == null)
                throw new Exception("Branch does not exist.");
        }

        user.FullName = request.FullName;
        user.Email = request.Email;
        user.Phone = request.Phone;
        user.RoleId = request.RoleId;
        user.BranchId = request.BranchId;
        user.IsActive = request.IsActive;

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync();

        return new UserDto
        {
            UserId = user.UserId,
            Username = user.Username,
            FullName = user.FullName,
            Email = user.Email,
            Phone = user.Phone,
            RoleId = user.RoleId,
            RoleName = role.RoleName,
            BranchId = user.BranchId,
            BranchName = branch?.BranchName,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt
        };
    }

    public async Task DeactivateUserAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new Exception("User not found.");

        user.IsActive = false;
        
        // Also invalidate their refresh token to force them out immediately if needed
        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync();
    }
}
