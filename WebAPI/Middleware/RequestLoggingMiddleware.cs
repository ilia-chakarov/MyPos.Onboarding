using System.Diagnostics;
using System.Text;

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

            var queryString = request.QueryString.HasValue ? request.QueryString.Value : string.Empty;
            var headers = request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString());

            context.Request.EnableBuffering();

            string requestBody = string.Empty;
            using (var reader = new StreamReader(
                context.Request.Body,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                bufferSize: 1024,
                leaveOpen: true
                ))
            {
                requestBody = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            // for the response
            var originalStream = context.Response.Body;
            using var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;
            _logger.LogInformation("==============================================================================================================================");
            _logger.LogInformation("=== HTTP REQUEST ===");
            _logger.LogInformation("Incoming request: {Method} {Path} {QueryString}", method, path, queryString);
            _logger.LogInformation("Request headers: {@Headers}", headers);
            _logger.LogInformation("Request body: {RequestBody}", requestBody);

            await _next(context);

            stopwatch.Stop();
            var statusCode = context.Response.StatusCode;

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            string responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            _logger.LogInformation("=== HTTP RESPONSE ===");
            _logger.LogInformation("Response body: {ResponseBody}", responseBody);
            _logger.LogInformation("Request completed: {Method} {Path} responded {StatusCode} in {Elapsed} ms",
                method, path, statusCode, stopwatch.ElapsedMilliseconds);

            await responseBodyStream.CopyToAsync(originalStream);
        }
    }
}
