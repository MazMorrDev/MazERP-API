using DotNetEnv;
using MazErpBack;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
Env.Load();
var connectionString = Env.GetString("POSTGRES_CONNECTION_STRING");

// Builder logic moved to dedicated class
WebAppBuilderConfig.ConfigureBuilder(builder, connectionString);
WebAppBuilderConfig.ConfigureCorsPolicy(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi("/openapi/{documentName}.json");
    app.MapScalarApiReference();
}

app.UseCors("AllowSpecificOrigin");
app.UseHttpsRedirection();
app.MapControllers();
app.Run();