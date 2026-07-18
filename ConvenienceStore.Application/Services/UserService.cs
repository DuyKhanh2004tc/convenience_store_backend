using ConvenienceStore.Application.Contracts.Persistence;
using ConvenienceStore.Application.DTOs.Common;
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

    public async Task<PagedResult<UserDto>> GetUsersAsync(QueryParameters query)
    {
        var usersQuery = await _userRepository.GetAllAsync();
        
        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            var search = query.SearchTerm.ToLower();
            usersQuery = usersQuery.Where(u => 
                u.FullName.ToLower().Contains(search) || 
                u.Email.ToLower().Contains(search) ||
                (u.Phone != null && u.Phone.Contains(search))
            ).ToList();
        }

        var totalCount = usersQuery.Count();
        
        var pagedUsers = usersQuery
            .OrderByDescending(u => u.CreatedAt)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();

        var roles = await _roleRepository.GetAllAsync();
        var branches = await _branchRepository.GetAllAsync();

        var userDtos = pagedUsers.Select(u => new UserDto
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
        }).ToList();

        return new PagedResult<UserDto>
        {
            Items = userDtos,
            TotalCount = totalCount,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize
        };
    }

    public async Task<UserStatsDto> GetUserStatsAsync()
    {
        var allUsers = await _userRepository.GetAllAsync();
        var allRoles = await _roleRepository.GetAllAsync();
        
        var storeManagerRole = allRoles.FirstOrDefault(r => r.RoleName.ToLower().Contains("manager"));
        var storeManagerRoleId = storeManagerRole?.RoleId ?? -1;

        return new UserStatsDto
        {
            TotalUsers = allUsers.Count(),
            ActiveUsers = allUsers.Count(u => u.IsActive),
            StoreManagers = allUsers.Count(u => u.RoleId == storeManagerRoleId),
            LeaveRequests = 14 // Mocked as per template for now
        };
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
            throw new Exception("Tên đăng nhập đã tồn tại.");

        // 2. Validate Phone uniqueness (BR-13)
        if (!string.IsNullOrEmpty(request.Phone))
        {
            var existingPhone = await _userRepository.GetAsync(u => u.Phone == request.Phone);
            if (existingPhone.Any())
                throw new Exception("Số điện thoại đã được sử dụng.");
        }

        // 3. Validate Email uniqueness
        if (!string.IsNullOrEmpty(request.Email))
        {
            var existingEmail = await _userRepository.GetAsync(u => u.Email == request.Email);
            if (existingEmail.Any())
                throw new Exception("Email đã được sử dụng.");
        }

        // 4. Validate Role
        var role = await _roleRepository.GetByIdAsync(request.RoleId);
        if (role == null)
            throw new Exception("Vai trò được chọn không tồn tại.");

        // 5. Validate Branch if provided
        Branch? branch = null;
        if (request.BranchId.HasValue)
        {
            branch = await _branchRepository.GetByIdAsync(request.BranchId.Value);
            if (branch == null)
                throw new Exception("Chi nhánh được chọn không tồn tại.");
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
                throw new Exception("Số điện thoại đã được sử dụng.");
        }

        // Validate Email uniqueness
        if (!string.IsNullOrEmpty(request.Email) && request.Email != user.Email)
        {
            var existingEmail = await _userRepository.GetAsync(u => u.Email == request.Email && u.UserId != id);
            if (existingEmail.Any())
                throw new Exception("Email đã được sử dụng.");
        }

        // Validate Role
        var role = await _roleRepository.GetByIdAsync(request.RoleId);
        if (role == null)
            throw new Exception("Vai trò được chọn không tồn tại.");

        // Validate Branch if provided
        Branch? branch = null;
        if (request.BranchId.HasValue)
        {
            branch = await _branchRepository.GetByIdAsync(request.BranchId.Value);
            if (branch == null)
                throw new Exception("Chi nhánh được chọn không tồn tại.");
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

    public async Task ActivateUserAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new Exception("User not found.");

        user.IsActive = true;
        
        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<string> ResetUserPasswordAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new Exception("User not found.");

        string newPassword = GenerateRandomPassword();
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);

        // Also invalidate their refresh token so they have to login with new password
        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync();

        return newPassword;
    }

    private string GenerateRandomPassword()
    {
        const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
        const string digits = "0123456789";
        const string specialChars = "!@#$%^&*";

        var random = new Random();
        var passwordChars = new char[8];

        // Ensure at least one of each required type
        passwordChars[0] = upperCase[random.Next(upperCase.Length)];
        passwordChars[1] = digits[random.Next(digits.Length)];
        passwordChars[2] = specialChars[random.Next(specialChars.Length)];

        const string allChars = upperCase + lowerCase + digits + specialChars;
        for (int i = 3; i < 8; i++)
        {
            passwordChars[i] = allChars[random.Next(allChars.Length)];
        }

        // Shuffle the characters
        for (int i = 0; i < passwordChars.Length; i++)
        {
            int swapIndex = random.Next(passwordChars.Length);
            var temp = passwordChars[i];
            passwordChars[i] = passwordChars[swapIndex];
            passwordChars[swapIndex] = temp;
        }

        return new string(passwordChars);
    }
}
