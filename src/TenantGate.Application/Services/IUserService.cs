namespace TenantGate.Application.Services;

public interface IUserService
{
    Task<IReadOnlyList<UserDto>> GetUsersAsync();

    Task<UserDto?> GetUserByIdAsync(Guid userId);

    Task<UserDto> CreateUserAsync(CreateUserRequest request);

    Task<UserDto?> UpdateUserAsync(Guid userId, UpdateUserRequest request);

    Task<bool> DeleteUserAsync(Guid userId);
}