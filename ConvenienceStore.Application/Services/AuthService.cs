using ConvenienceStore.Application.Contracts.External;
using ConvenienceStore.Application.Contracts.Persistence;
using ConvenienceStore.Application.DTOs.Auth;
using ConvenienceStore.Application.Services.Interfaces;
using ConvenienceStore.Domain.Entities;

namespace ConvenienceStore.Application.Services;

public class AuthService : IAuthService
{
    private readonly IGenericRepository<User> _userRepository;
    private readonly IGenericRepository<Role> _roleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(
        IGenericRepository<User> userRepository,
        IGenericRepository<Role> roleRepository,
        IUnitOfWork unitOfWork,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        // 1. Find user by username
        var users = await _userRepository.GetAsync(x => x.Username == request.Username && x.IsActive);
        var user = users.FirstOrDefault();
        if (user == null)
        {
            throw new Exception("Invalid username or password"); // In real app, throw Custom Exception (e.g. UnauthorizedException)
        }

        // 2. Verify password
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if (!isPasswordValid)
        {
            throw new Exception("Invalid username or password");
        }

        // 3. Get Role name
        var role = await _roleRepository.GetByIdAsync(user.RoleId);
        if (role == null)
        {
            throw new Exception("Role not found");
        }

        // 4. Generate JWT Token
        var token = _jwtTokenGenerator.GenerateToken(user, role.RoleName);

        return new AuthResponse
        {
            UserId = user.UserId,
            Username = user.Username,
            FullName = user.FullName,
            RoleName = role.RoleName,
            Token = token
        };
    }

    public async Task<AuthResponse> SeedAdminAsync()
    {
        // Create System Admin role if not exists
        var roles = await _roleRepository.GetAsync(r => r.RoleName == "System Admin");
        var adminRole = roles.FirstOrDefault();

        if (adminRole == null)
        {
            adminRole = new Role
            {
                RoleName = "System Admin",
                Description = "Administrator with full access"
            };
            await _roleRepository.AddAsync(adminRole);
            await _unitOfWork.SaveChangesAsync(); // Save to get RoleId
        }

        // Check if admin user exists
        var users = await _userRepository.GetAsync(u => u.Username == "admin");
        var adminUser = users.FirstOrDefault();

        if (adminUser == null)
        {
            adminUser = new User
            {
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"), // Seed with default password
                FullName = "System Administrator",
                RoleId = adminRole.RoleId,
                IsActive = true
            };
            await _userRepository.AddAsync(adminUser);
            await _unitOfWork.SaveChangesAsync();
        }

        // Return token
        var token = _jwtTokenGenerator.GenerateToken(adminUser, adminRole.RoleName);

        return new AuthResponse
        {
            UserId = adminUser.UserId,
            Username = adminUser.Username,
            FullName = adminUser.FullName,
            RoleName = adminRole.RoleName,
            Token = token
        };
    }
}
