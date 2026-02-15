using MazErpBack.Context;
using MazErpBack.Enums;
using MazErpBack.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MazErpBack.Services.Implementation;

public class RoleAuthorizationService(AppDbContext context, ILogger<RoleAuthorizationService> logger) : IRoleAuthorizationService
{
    private readonly AppDbContext _context = context;
    private readonly ILogger<RoleAuthorizationService> _logger = logger;

    public async Task<bool> UserHasAccessAsync(int userId, int companyId, string zone, CancellationToken cancellationToken = default)
    {
        try
        {
            var userCompany = await _context.UserCompanies
                .Include(uc => uc.User)
                .Include(uc => uc.Company)
                .FirstOrDefaultAsync(uc =>
                    uc.UserId == userId &&
                    uc.CompanyId == companyId &&
                    uc.IsActive,
                    cancellationToken);

            if (userCompany == null)
            {
                _logger.LogWarning("Usuario {UserId} no tiene acceso a la compañía {CompanyId}", userId, companyId);
                return false;
            }

            // Aquí se define qué roles tienen acceso a qué zonas
            var hasAccess = userCompany.Role switch
            {
                // Owner tiene acceso a todo
                UserCompanyRole.Owner => true,

                // Admin tiene acceso a todo menos al /api/Company/Delete
                UserCompanyRole.Admin => 
                zone.StartsWith("/api/Company/") || zone.StartsWith("/api/Inventory"),

                // Aquí algunos ejemplos que dejó la IA que después cambiaré por los roles reales, por ahora solo admin y owner para probar
                
                // // Gerente puede acceder a zonas de gestión
                // UserCompanyRole.Manager => zone.StartsWith("/api/management") ||
                //                           zone.StartsWith("/api/reports"),

                // // Vendedor solo a zonas de ventas
                // UserCompanyRole.Seller => zone.StartsWith("/api/sales") ||
                //                          zone.StartsWith("/api/sellpoints"),

                // // Almacenero solo a zonas de inventario
                // UserCompanyRole.Warehouse => zone.StartsWith("/api/inventory") ||
                //                             zone.StartsWith("/api/warehouses"),

                // // Auditor solo a zonas de solo lectura
                // UserCompanyRole.Auditor => zone.StartsWith("/api/reports") ||
                //                           zone.StartsWith("/api/audit"),

                // Por defecto, denegar acceso
                _ => false
            };

            if (!hasAccess)
            {
                _logger.LogWarning("Usuario {UserId} con rol {Role} intentó acceder a zona no permitida: {Zone}",
                    userId, userCompany.Role, zone);
            }

            return hasAccess;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verificando acceso para usuario {UserId} en compañía {CompanyId}", userId, companyId);
            return false;
        }
    }

    public async Task<UserCompanyRole?> GetUserRoleInCompanyAsync(int userId, int companyId, CancellationToken cancellationToken = default)
    {
        var userCompany = await _context.UserCompanies.FirstOrDefaultAsync(uc =>
                uc.UserId == userId &&
                uc.CompanyId == companyId &&
                uc.IsActive,
                cancellationToken);

        return userCompany?.Role;
    }
}
