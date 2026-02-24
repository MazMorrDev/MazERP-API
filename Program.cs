using DotNetEnv;
using MazErpBack.Config;

var builder = WebApplication.CreateBuilder(args);
Env.Load();
var environment = Environment.GetEnvironmentVariable("MY_APP_ENVIRONMENT") ?? "Production"; // Development o Production 

// Ahora tú controlas todo explícitamente
var connectionString = environment switch
{
    "Development" => Environment.GetEnvironmentVariable("POSTGRES_LOCAL_CONNECTION_STRING"),
    "Production" => Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING"),
    _ => Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING")
} ?? throw new InvalidOperationException("No valid connection string found.");

var jwtSecret = Env.GetString("JWT_SECRET") ?? throw new InvalidOperationException("JWT_SECRET environment variable is not set.");
var port = Env.GetString("PORT", "5148") ?? throw new InvalidOperationException("PORT environment variable is not set.");

// Builder logic moved to dedicated class
WebAppBuilderConfig.ConfigureBuilder(builder, connectionString, int.Parse(port));
WebAppBuilderConfig.ConfigureCorsPolicy(builder);
WebAppBuilderConfig.ConfigureAuthentication(builder, jwtSecret);

var app = builder.Build();

// App logic moved to dedicated class
WebAppConfig.UseGeneralApiConfigs(app);

app.Run();
