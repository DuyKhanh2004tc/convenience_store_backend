using ConvenienceStore.Domain.Entities;

namespace ConvenienceStore.Application.Contracts.External;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user, string roleName);
}
