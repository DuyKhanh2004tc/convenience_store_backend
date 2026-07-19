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
    private readonly IGenericRepository<Branch> _branchRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ConvenienceStore.Application.Contracts.External.ICacheService _cacheService;
    private readonly IEmailService _emailService;

    public AuthService(
        IGenericRepository<User> userRepository,
        IGenericRepository<Role> roleRepository,
        IGenericRepository<Branch> branchRepository,
        IUnitOfWork unitOfWork,
        IJwtTokenGenerator jwtTokenGenerator,
        ConvenienceStore.Application.Contracts.External.ICacheService cacheService,
        IEmailService emailService)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _branchRepository = branchRepository;
        _unitOfWork = unitOfWork;
        _jwtTokenGenerator = jwtTokenGenerator;
        _cacheService = cacheService;
        _emailService = emailService;
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var users = await _userRepository.GetAsync(x => x.Username == request.Username && x.IsActive);
        var user = users.FirstOrDefault();
        if (user == null)
            throw new Exception("Invalid username or password");

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if (!isPasswordValid)
            throw new Exception("Invalid username or password");

        // Check if the user's branch is still active
        if (user.BranchId.HasValue)
        {
            var branch = await _branchRepository.GetByIdAsync(user.BranchId.Value);
            if (branch != null && !branch.IsActive)
                throw new Exception("Chi nhánh đã ngừng hoạt động. Vui lòng liên hệ quản trị viên.");
        }

        var role = await _roleRepository.GetByIdAsync(user.RoleId);
        if (role == null)
            throw new Exception("Role not found");

        var tokens = _jwtTokenGenerator.GenerateTokens(user, role.RoleName);
        
        user.RefreshToken = tokens.RefreshToken;
        user.RefreshTokenExpiryTime = tokens.RefreshTokenExpiryTime;

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync();

        return new AuthResponse
        {
            UserId = user.UserId,
            Username = user.Username,
            FullName = user.FullName,
            RoleName = role.RoleName,
            BranchId = user.BranchId,
            Token = tokens.AccessToken,
            RefreshToken = tokens.RefreshToken
        };
    }

    public async Task<AuthResponse> SeedAdminAsync()
    {
        var defaultRoles = new List<string> { "System Admin", "Chain Director", "Store Manager", "Store Staff", "Warehouse Staff" };
        var existingRoles = await _roleRepository.GetAllAsync();
        
        foreach (var roleName in defaultRoles)
        {
            if (!existingRoles.Any(r => r.RoleName == roleName))
            {
                await _roleRepository.AddAsync(new Role
                {
                    RoleName = roleName,
                    Description = $"{roleName} role"
                });
            }
        }
        await _unitOfWork.SaveChangesAsync();

        var adminRole = (await _roleRepository.GetAsync(r => r.RoleName == "System Admin")).FirstOrDefault();
        if (adminRole == null) throw new Exception("Admin role not found after seeding.");

        var users = await _userRepository.GetAsync(u => u.Username == "admin");
        var adminUser = users.FirstOrDefault();

        if (adminUser == null)
        {
            adminUser = new User
            {
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                FullName = "System Administrator",
                RoleId = adminRole.RoleId,
                IsActive = true
            };
            await _userRepository.AddAsync(adminUser);
            await _unitOfWork.SaveChangesAsync();
        }

        var tokens = _jwtTokenGenerator.GenerateTokens(adminUser, adminRole.RoleName);
        
        adminUser.RefreshToken = tokens.RefreshToken;
        adminUser.RefreshTokenExpiryTime = tokens.RefreshTokenExpiryTime;

        _userRepository.Update(adminUser);
        await _unitOfWork.SaveChangesAsync();

        return new AuthResponse
        {
            UserId = adminUser.UserId,
            Username = adminUser.Username,
            FullName = adminUser.FullName,
            RoleName = adminRole.RoleName,
            BranchId = adminUser.BranchId,
            Token = tokens.AccessToken,
            RefreshToken = tokens.RefreshToken
        };
    }

    public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var principal = _jwtTokenGenerator.GetPrincipalFromExpiredToken(request.AccessToken);
        if (principal == null)
            throw new Exception("Invalid access token or refresh token");

        var userIdClaim = principal.FindFirst("sub")?.Value;
        if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            throw new Exception("Invalid access token or refresh token");

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            throw new Exception("Invalid access token or refresh token");

        var role = await _roleRepository.GetByIdAsync(user.RoleId);
        
        var tokens = _jwtTokenGenerator.GenerateTokens(user, role!.RoleName);
        
        user.RefreshToken = tokens.RefreshToken;
        user.RefreshTokenExpiryTime = tokens.RefreshTokenExpiryTime;

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync();

        return new AuthResponse
        {
            UserId = user.UserId,
            Username = user.Username,
            FullName = user.FullName,
            RoleName = role.RoleName,
            BranchId = user.BranchId,
            Token = tokens.AccessToken,
            RefreshToken = tokens.RefreshToken
        };
    }

    public async Task LogoutAsync(int userId, string jti, TimeSpan remainingTime)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new Exception("User not found");

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;
        
        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync();

        if (remainingTime.TotalSeconds > 0)
        {
            await _cacheService.SetAsync($"blacklist:{jti}", "true", remainingTime);
        }
    }

    public async Task ForgotPasswordAsync(ForgotPasswordRequestDto request)
    {
        var users = await _userRepository.GetAsync(x => x.Email == request.Email && x.IsActive);
        var user = users.FirstOrDefault();
        
        if (user == null)
            throw new Exception("User not found or inactive.");

        var random = new Random();
        var otp = random.Next(100000, 999999).ToString();

        user.PasswordResetToken = otp;
        user.PasswordResetTokenExpires = DateTime.UtcNow.AddMinutes(15);

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync();

        var subject = "Your Password Reset OTP";
        var body = $"<p>Hi {user.FullName},</p><p>Your OTP to reset password is: <strong>{otp}</strong>. It will expire in 15 minutes.</p>";
        await _emailService.SendEmailAsync(user.Email!, subject, body);
    }

    public async Task ResetPasswordAsync(ResetPasswordRequestDto request)
    {
        var users = await _userRepository.GetAsync(x => x.Email == request.Email && x.IsActive);
        var user = users.FirstOrDefault();

        if (user == null)
            throw new Exception("User not found or inactive.");

        if (user.PasswordResetToken != request.Otp || user.PasswordResetTokenExpires < DateTime.UtcNow)
        {
            throw new Exception("Invalid or expired OTP.");
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        user.PasswordResetToken = null;
        user.PasswordResetTokenExpires = null;

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync();
    }
}
