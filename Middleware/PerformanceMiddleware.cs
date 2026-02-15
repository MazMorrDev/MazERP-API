using System.Diagnostics;

namespace MazErpBack.Middleware;

public class PerformanceMiddleware(RequestDelegate next, ILogger<PerformanceMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<PerformanceMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        
        try
        {
            await _next(context);
        }
        finally
        {
            sw.Stop();
            
            _logger.LogInformation(
                "Request {Method} {Path} completado en {ElapsedMs}ms con status {StatusCode}",
                context.Request.Method,
                context.Request.Path,
                sw.ElapsedMilliseconds,
                context.Response.StatusCode
            );
        }
    }
}