using ConvenienceStore.Application.DTOs.User;

namespace ConvenienceStore.Application.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto?> GetUserByIdAsync(int id);
    Task<UserDto> CreateUserAsync(CreateUserDto request);
    Task<UserDto> UpdateUserAsync(int id, UpdateUserDto request);
    Task DeactivateUserAsync(int id);
}
