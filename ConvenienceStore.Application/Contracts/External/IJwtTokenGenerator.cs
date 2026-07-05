using System.Security.Claims;
using ConvenienceStore.Application.DTOs.Auth;
using ConvenienceStore.Domain.Entities;

namespace ConvenienceStore.Application.Contracts.External;

public interface IJwtTokenGenerator
{
    TokenDto GenerateTokens(User user, string roleName);
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}
