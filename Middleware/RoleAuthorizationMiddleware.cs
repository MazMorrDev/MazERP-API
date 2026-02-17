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
        "/api/user/company/create",
        "/api/user/company/by-user",
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

        // 4. Obtener companyId del header
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
        if (context.Request.Headers.TryGetValue("companyId", out var companyIdValue))
        {
            if (int.TryParse(companyIdValue.FirstOrDefault(), out companyId))
                return true;
        }
        return false;
    }

    private static string DetermineZone(HttpContext context)
    {
        // Determinar por el path
        var path = context.Request.Path.Value?.ToLower() ?? string.Empty;

        if (path.StartsWith("/api/sell")) return "/api/sell";
        if (path.StartsWith("/api/buy")) return "/api/buy";
        if (path.StartsWith("/api/inventory")) return "/api/inventory";
        if (path.StartsWith("/api/report")) return "/api/report";
        if (path.StartsWith("/api/admin")) return "/api/admin";
        if (path.StartsWith("/api/warehouse")) return "/api/warehouse";
        if (path.StartsWith("/api/sellpoint")) return "/api/sellpoint";
        if (path.StartsWith("/api/company")) return "/api/company";
        if (path.StartsWith("/api/user")) return "/api/user";

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