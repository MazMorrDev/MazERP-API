using DotNetEnv;
using MazErpBack;

var builder = WebApplication.CreateBuilder(args);
Env.Load();
var connectionString = Env.GetString("POSTGRES_CONNECTION_STRING");
var jwtSecret = Env.GetString("JWT_SECRET");
var port = Env.GetString("PORT", "5148");

// Builder logic moved to dedicated class
WebAppBuilderConfig.ConfigureBuilder(builder, connectionString, int.Parse(port));
WebAppBuilderConfig.ConfigureCorsPolicy(builder);
WebAppBuilderConfig.ConfigureAuthentication(builder, jwtSecret);

var app = builder.Build();

// App logic moved to dedicated class
WebAppConfig.UseDevApiConfigs(app);
WebAppConfig.UseGeneralApiConfigs(app);
