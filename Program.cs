using DotNetEnv;
using MazErpBack.Config;

var builder = WebApplication.CreateBuilder(args);
Env.Load();
var connectionString = Env.GetString("POSTGRES_CONNECTION_STRING") ?? throw new InvalidOperationException("POSTGRES_CONNECTION_STRING environment variable is not set.");
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
