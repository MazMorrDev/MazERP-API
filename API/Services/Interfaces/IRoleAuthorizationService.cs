using MazErpAPI.Enums;

namespace MazErpAPI.Services.Interfaces;

public interface IRoleAuthorizationService
{
    Task<bool> UserHasAccessAsync(int userId, int companyId, string zone, CancellationToken cancellationToken = default);
    Task<UserCompanyRole?> GetUserRoleInCompanyAsync(int userId, int companyId, CancellationToken cancellationToken = default);
}
