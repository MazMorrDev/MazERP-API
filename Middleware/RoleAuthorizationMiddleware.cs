using System.Security.Claims;
using System.Text.Json;
using MazErpBack.Services.Interfaces;

namespace MazErpBack.Middleware;

public class RoleAuthorizationMiddleware(RequestDelegate next, ILogger<RoleAuthorizationMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<RoleAuthorizationMiddleware> _logger = logger;

    // Lista de rutas que no requieren autorización
    private readonly string[] _publicPaths =
    [
        "/api/user/register",
        "/api/user/login",
        "/swagger",
        "/openapi",
        "/health",
        "/"
    ];

    public async Task InvokeAsync(HttpContext context, IRoleAuthorizationService authService)
    {
        // 1. Verificar si es una ruta pública
        var path = context.Request.Path.Value?.ToLower() ?? string.Empty;
        if (_publicPaths.Any(p => path.StartsWith(p, StringComparison.CurrentCultureIgnoreCase)))
        {
            await _next(context);
            return;
        }

        // 2. Verificar si el usuario está autenticado
        if (!context.User.Identity?.IsAuthenticated ?? true)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await WriteErrorResponse(context, "Usuario no autenticado");
            return;
        }

        // 3. Obtener información del usuario
        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdClaim, out var userId))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await WriteErrorResponse(context, "Token inválido - usuario no identificado");
            return;
        }

        // 4. Obtener companyId del header o del token
        if (!TryGetCompanyId(context, out var companyId))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await WriteErrorResponse(context, "Company ID no proporcionado");
            return;
        }

        // 5. Determinar la zona de la petición
        var zone = DetermineZone(context);

        // 6. Verificar si la zona requiere autorización
        if (string.IsNullOrEmpty(zone))
        {
            // Si no tiene atributo de zona, continuar (podrías cambiar esto según tu política)
            await _next(context);
            return;
        }

        // 7. Verificar acceso
        var hasAccess = await authService.UserHasAccessAsync(userId, companyId, zone);

        if (!hasAccess)
        {
            var role = await authService.GetUserRoleInCompanyAsync(userId, companyId);
            _logger.LogWarning("Acceso denegado - Usuario {UserId} con rol {Role} intentó acceder a zona {Zone}",
                userId, role, zone);

            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await WriteErrorResponse(context, "No tiene permisos suficientes para acceder a este recurso");
            return;
        }

        // 8. Guardar información en Items para uso posterior
        context.Items["UserId"] = userId;
        context.Items["CompanyId"] = companyId;

        var userRole = await authService.GetUserRoleInCompanyAsync(userId, companyId);
        context.Items["UserRole"] = userRole?.ToString();

        // 9. Continuar con el pipeline
        await _next(context);
    }

    private static bool TryGetCompanyId(HttpContext context, out int companyId)
    {
        companyId = 0;

        // Intentar obtener del header
        if (context.Request.Headers.TryGetValue("X-Company-Id", out var companyIdValue) ||
            context.Request.Headers.TryGetValue("Company-Id", out companyIdValue))
        {
            if (int.TryParse(companyIdValue.FirstOrDefault(), out companyId))
                return true;
        }

        // Intentar obtener del token JWT (si lo tienes como claim)
        var companyClaim = context.User.FindFirst("CompanyId")?.Value;
        if (!string.IsNullOrEmpty(companyClaim) && int.TryParse(companyClaim, out companyId))
            return true;

        // Intentar obtener de query string
        if (context.Request.Query.TryGetValue("companyId", out var queryValue) ||
            context.Request.Query.TryGetValue("company_id", out queryValue))
        {
            if (int.TryParse(queryValue.FirstOrDefault(), out companyId))
                return true;
        }

        return false;
    }

    private static string DetermineZone(HttpContext context)
    {
        // Determinar por el path
        var path = context.Request.Path.Value?.ToLower() ?? string.Empty;

        if (path.StartsWith("/api/sales")) return "/api/sales";
        if (path.StartsWith("/api/inventory")) return "/api/inventory";
        if (path.StartsWith("/api/reports")) return "/api/reports";
        if (path.StartsWith("/api/admin")) return "/api/admin";
        if (path.StartsWith("/api/warehouses")) return "/api/warehouses";
        if (path.StartsWith("/api/sellpoints")) return "/api/sellpoints";
        if (path.StartsWith("/api/companies")) return "/api/companies";
        if (path.StartsWith("/api/users")) return "/api/users";

        return string.Empty;
    }

    private static async Task WriteErrorResponse(HttpContext context, string message)
    {
        context.Response.ContentType = "application/json";
        var response = new
        {
            success = false,
            message,
            timestamp = DateTime.UtcNow,
            path = context.Request.Path
        };

        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }
}