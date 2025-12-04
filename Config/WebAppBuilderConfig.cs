using MazErpBack.Services.User;
using Microsoft.EntityFrameworkCore;

namespace MazErpBack;

public class WebAppBuilderConfig
{

    public static void ConfigureBuilder(WebApplicationBuilder builder, string connectionString)
    {
        // Add services to the container.
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddOpenApi("v1"); // Customize the document name
        builder.Services.AddDbContext<AppDbContext>(optionsAction: options => options.UseNpgsql(connectionString));
        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<TokenService>();
        // TODO: map and register Mapster DI
        builder.Services.AddControllers();

    }

    public static void ConfigureCorsPolicy(WebApplicationBuilder builder)
    {
                // Configure CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin", policy =>
            {
                policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
            });
        });
    }
}
