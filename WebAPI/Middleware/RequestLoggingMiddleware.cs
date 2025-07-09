using System.Diagnostics;

namespace WebAPI.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            var request = context.Request;
            var method = request.Method;
            var path = request.Path;

            _logger.LogInformation("Incoming request: {Method} {path}", method, path);

            await _next(context);

            stopwatch.Stop();
            var statusCode = context.Response.StatusCode;

            _logger.LogInformation("Request completed: {Method} {Path} responded {StatusCode} in {Elapsed} ms",
                method, path, statusCode, stopwatch.ElapsedMilliseconds);
        }
    }
}
