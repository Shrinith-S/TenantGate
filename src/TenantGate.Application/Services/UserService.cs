using TenantGate.Application.Context;

namespace TenantGate.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ITenantRepository _tenantRepository;
    private readonly ITenantContext _tenantContext;

    public UserService(
        IUserRepository userRepository,
        ITenantRepository tenantRepository,
        ITenantContext tenantContext)
    {
        _userRepository = userRepository;
        _tenantRepository = tenantRepository;
        _tenantContext = tenantContext;
    }

    public async Task<IReadOnlyList<UserDto>> GetUsersAsync()
    {
        var entraTenantId =
            _tenantContext.EntraTenantId.ToString();

        var tenant = await _tenantRepository
            .GetActiveTenantByEntraTenantIdAsync(entraTenantId);

        if (tenant == null)
        {
            throw new UnauthorizedAccessException(
                "Tenant is not registered or inactive.");
        }

        var users = await _userRepository
            .GetActiveAsync();

        return users
            .Select(x => new UserDto
            {
                Id = x.Id,
                EntraObjectId = x.EntraObjectId,
                Email = x.Email,
                DisplayName = x.DisplayName,
                IsActive = x.IsActive
            })
            .ToList();
    }

    public async Task<UserDto?> GetUserByIdAsync(Guid userId)
    {
        var entraTenantId =
            _tenantContext.EntraTenantId.ToString();

        var tenant = await _tenantRepository
            .GetActiveTenantByEntraTenantIdAsync(entraTenantId);

        if (tenant == null)
        {
            throw new UnauthorizedAccessException(
                "Tenant is not registered or inactive.");
        }

        var user = await _userRepository
            .GetByIdAsync(userId);

        if (user == null)
        {
            return null;
        }

        return new UserDto
        {
            Id = user.Id,
            EntraObjectId = user.EntraObjectId,
            Email = user.Email,
            DisplayName = user.DisplayName,
            IsActive = user.IsActive
        };
    }

    public async Task<UserDto> CreateUserAsync(CreateUserRequest request)
    {
        var entraTenantId = _tenantContext.EntraTenantId.ToString();

        var tenant = await _tenantRepository.GetActiveTenantByEntraTenantIdAsync(entraTenantId);

        if (tenant == null)
        {
            throw new UnauthorizedAccessException("Tenant is not registered or inactive.");
        }

        var existingUser = await _userRepository.GetByEntraObjectIdAsync(request.EntraObjectId);

        if (existingUser != null)
        {
            throw new InvalidOperationException("User already exists in this tenant.");
        }

        var user = new Domain.Entities.User
        {
            Id = Guid.NewGuid(),

            // 🔐 IMPORTANT:
            // Tenant comes from authenticated context
            TenantId = tenant.Id,

            EntraObjectId = request.EntraObjectId,
            Email = request.Email,
            DisplayName = request.DisplayName,
            IsActive = true
        };

        // We need a repository method to persist the user.
        // We'll add that next.

        await _userRepository.AddAsync(user);

        return new UserDto
        {
            Id = user.Id,
            EntraObjectId = user.EntraObjectId,
            Email = user.Email,
            DisplayName = user.DisplayName,
            IsActive = user.IsActive
        };
    }

    public async Task<UserDto?> UpdateUserAsync(
    Guid userId,
    UpdateUserRequest request)
    {
        var entraTenantId =
            _tenantContext.EntraTenantId.ToString();

        var tenant = await _tenantRepository
            .GetActiveTenantByEntraTenantIdAsync(entraTenantId);

        if (tenant == null)
        {
            throw new UnauthorizedAccessException(
                "Tenant is not registered or inactive.");
        }

        var user = await _userRepository
            .GetByIdAsync(userId);

        if (user == null)
        {
            return null;
        }

        user.Email = request.Email;
        user.DisplayName = request.DisplayName;

        await _userRepository.UpdateAsync(user);

        return new UserDto
        {
            Id = user.Id,
            EntraObjectId = user.EntraObjectId,
            Email = user.Email,
            DisplayName = user.DisplayName,
            IsActive = user.IsActive
        };
    }

    public async Task<bool> DeleteUserAsync(Guid userId)
    {
        var entraTenantId =
            _tenantContext.EntraTenantId.ToString();

        var tenant = await _tenantRepository
            .GetActiveTenantByEntraTenantIdAsync(entraTenantId);

        if (tenant == null)
        {
            throw new UnauthorizedAccessException(
                "Tenant is not registered or inactive.");
        }

        var user = await _userRepository
            .GetByIdAsync(userId);

        if (user == null)
        {
            return false;
        }

        user.IsActive = false;

        await _userRepository.UpdateAsync(user);

        return true;
    }
}