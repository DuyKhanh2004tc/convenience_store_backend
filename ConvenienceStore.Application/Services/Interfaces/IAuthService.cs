using ConvenienceStore.Application.DTOs.Auth;

namespace ConvenienceStore.Application.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> SeedAdminAsync();
    Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request);
}
