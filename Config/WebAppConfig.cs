using MazErpBack.Middleware;
using Scalar.AspNetCore;

namespace MazErpBack.Config;

public class WebAppConfig
{
    public static void UseGeneralApiConfigs(WebApplication app)
    {
        UseDevApiConfigs(app);

        // 1. CORS
        app.UseCors("AllowSpecificOrigin");

        // 2. Redirección HTTPS
        app.UseHttpsRedirection();

        // 3. Archivos estáticos (si tienes)
        // app.UseStaticFiles();

        // 4. Routing 
        app.UseRouting();

        // 5. Autenticación
        app.UseAuthentication();

        // 6. Middleware de performance 
        app.UseDeveloperExceptionPage();

        // 7. Middleware de autorización personalizado 
        app.UseMiddleware<RoleAuthorizationMiddleware>();

        // 8. Autorización de ASP.NET
        app.UseAuthorization();

        // 9. Mapear los endpoints
        app.MapControllers();
    }

    public static void UseDevApiConfigs(WebApplication app)
    {
        // Solo configuraciones de desarrollo (OpenAPI, Scalar, etc)
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi("/openapi/v1.json");
            app.MapScalarApiReference(options =>
            {
                options.WithOpenApiRoutePattern("/openapi/v1.json");
            });
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/error");
            app.UseHsts();
        }
    }
}