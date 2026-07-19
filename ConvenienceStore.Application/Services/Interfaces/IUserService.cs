using ConvenienceStore.Application.DTOs.User;

using ConvenienceStore.Application.DTOs.Common;

namespace ConvenienceStore.Application.Services.Interfaces;

public interface IUserService
{
    Task<PagedResult<UserDto>> GetUsersAsync(QueryParameters query);
    Task<UserStatsDto> GetUserStatsAsync();
    Task<UserDto?> GetUserByIdAsync(int id);
    Task<UserDto> CreateUserAsync(CreateUserDto request);
    Task<UserDto> UpdateUserAsync(int id, UpdateUserDto request);
    Task DeactivateUserAsync(int id);
    Task ActivateUserAsync(int id);
    Task DeleteUserAsync(int id);
    Task<string> ResetUserPasswordAsync(int id);
}
