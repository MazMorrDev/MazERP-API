using DotNetEnv;
using MazErpBack;
using Scalar.AspNetCore;

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi("/openapi/v1.json");
    app.MapScalarApiReference( options =>
    {
        options.WithOpenApiRoutePattern("/openapi/v1.json");
    });
}

app.UseCors("AllowSpecificOrigin");
app.UseHttpsRedirection();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
app.Run();