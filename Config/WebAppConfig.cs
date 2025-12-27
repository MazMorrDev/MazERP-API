using Scalar.AspNetCore;

namespace MazErpBack;

public class WebAppConfig
{
    public static void UseGeneralApiConfigs(WebApplication app)
    {
        app.UseCors("AllowSpecificOrigin");
        app.UseHttpsRedirection();
        app.MapControllers();
        app.UseAuthentication();
        app.UseAuthorization();
        app.Run();
    }

    public static void UseDevApiConfigs(WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi("/openapi/v1.json");
            app.MapScalarApiReference(options =>
            {
                options.WithOpenApiRoutePattern("/openapi/v1.json");
            });
        }

    }
}
