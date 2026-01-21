using System.Security.Claims;
using MazErpBack.Context;
using MazErpBack.Services;
using MazErpBack.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace MazErpBack.Config;

public class WebAppBuilderConfig
{

    public static void ConfigureBuilder(WebApplicationBuilder builder, string connectionString, int port)
    {
        builder.WebHost.UseUrls($"http://localhost:{port}");
        // Add services to the container.
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddOpenApi("v1"); // Customize the document name
        builder.Services.AddDbContext<AppDbContext>(optionsAction: options => options.UseNpgsql(connectionString));
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IWorkflowService, WfService>();
        builder.Services.AddScoped<IWarehouseService, WarehouseService>();
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<IInventoryService, InventoryService>();
        // TODO: map and register Mapster DI
        builder.Services.AddControllers();

        builder.Services.AddAuthorizationBuilder()
            .AddPolicy("Client", policy => policy.RequireClaim(ClaimTypes.Role, "Client"))
            .AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"))
            .AddPolicy("Inventory", policy => policy.RequireClaim(ClaimTypes.Role, "Inventory"))
            .AddPolicy("Sales", policy => policy.RequireClaim(ClaimTypes.Role, "Sales"))
            .AddPolicy("Finance", policy => policy.RequireClaim(ClaimTypes.Role, "Finance"));
    }

    public static void ConfigureCorsPolicy(WebApplicationBuilder builder)
    {
        // Configure CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin", policy =>
            {
                policy.WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }

    public static void ConfigureAuthentication(WebApplicationBuilder builder, string jwtSecret)
    {
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "http://localhost", // add actual emitter by env
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSecret)),
            };
        });
    }
}
