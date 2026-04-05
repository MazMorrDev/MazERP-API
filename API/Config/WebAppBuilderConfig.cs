using System.Security.Claims;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Threading.RateLimiting;
using MazErpAPI.Services.Implementation;
using MazErpAPI.Services.Interfaces;
using MazErpAPI.Context;
using MazErpAPI.Utils.Mappers;

namespace MazErpAPI.Config;

public class WebAppBuilderConfig
{

    public static void ConfigureBuilder(WebApplicationBuilder builder, string connectionString, int port)
    {
        builder.WebHost.UseUrls($"http://localhost:{port}");
        // Add services to the container.
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddOpenApi("v1"); // Customize the document name

        ConfigureRateLimit(builder);

        builder.Services.AddDbContext<AppDbContext>(optionsAction: options => options.UseNpgsql(connectionString));

        AddServicesToScope(builder);
        AddMappersToScope(builder);

        builder.Services.AddControllers();

        builder.Services.AddAuthorizationBuilder()
            .AddPolicy("User", policy => policy.RequireClaim(ClaimTypes.Role, "user"));

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        builder.Host.UseSerilog();
    }

    public static void AddMappersToScope(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<UserMapper>();
        builder.Services.AddScoped<CompanyMapper>();
        builder.Services.AddScoped<WarehouseMapper>();
        builder.Services.AddScoped<ProductMapper>();
        builder.Services.AddScoped<InventoryMapper>();
        builder.Services.AddScoped<BuyMapper>();
        builder.Services.AddScoped<DevolutionMapper>();
        builder.Services.AddScoped<SellMapper>();
        builder.Services.AddScoped<SupplierMapper>();
        builder.Services.AddScoped<ExpenseMapper>();
        builder.Services.AddScoped<SellPointMapper>();
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

    public static void AddServicesToScope(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<ICompanyService, CompanyService>();
        builder.Services.AddScoped<IWarehouseService, WarehouseService>();
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<IInventoryService, InventoryService>();
        builder.Services.AddScoped<IBuyService, BuyService>();
        builder.Services.AddScoped<IDevolutionService, DevolutionService>();
        builder.Services.AddScoped<ISellService, SellService>();
        builder.Services.AddScoped<ISupplierService, SupplierService>();
        builder.Services.AddScoped<IExpenseService, ExpenseService>();
        builder.Services.AddScoped<ISellPointService, SellPointService>();
        builder.Services.AddScoped<IRoleAuthorizationService, RoleAuthorizationService>();
    }

    public static void ConfigureRateLimit(WebApplicationBuilder builder)
    {
        builder.Services.AddRateLimiter(options =>
        {
            // Configuración global
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(
                httpContext => RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: partition => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = 10,        // Máximo de solicitudes
                        Window = TimeSpan.FromSeconds(10),  // Ventana de tiempo
                        QueueLimit = 2,           // Solicitudes en cola
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                    }
                )
            );

            // Respuesta cuando se excede el límite
            options.OnRejected = async (context, token) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.HttpContext.Response.ContentType = "application/json";

                var response = new
                {
                    error = "Demasiadas solicitudes. Por favor, intenta de nuevo más tarde.",
                    retryAfter = context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter)
                        ? retryAfter.ToString()
                        : "10 segundos"
                };

                await context.HttpContext.Response.WriteAsJsonAsync(response, token);
            };
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
                ValidIssuer = "MazErpBack", // add actual emitter by env
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSecret)),
            };
        });
    }
}
