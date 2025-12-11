using MazErpBack.Services.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace MazErpBack;

public class WebAppBuilderConfig
{

    public static void ConfigureBuilder(WebApplicationBuilder builder, string connectionString)
    {
        // Add services to the container.
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
