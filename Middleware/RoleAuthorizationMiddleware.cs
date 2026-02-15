namespace MazErpBack.Middleware;

public class RoleAuthorizationMiddleware(ILogger<RoleAuthorizationMiddleware> logger)
{
    private readonly ILogger<RoleAuthorizationMiddleware> _logger = logger;
}
