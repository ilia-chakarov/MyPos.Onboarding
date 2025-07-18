using MyPos.Services.DTOs.Log;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

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
            var sanitizedBody = SanitizeRequestBody(path, requestBody);

            // for the response
            var originalStream = context.Response.Body;
            using var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;

            await _next(context);

            stopwatch.Stop();
            var statusCode = context.Response.StatusCode;

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            string responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            var logentry = new HttpLogEntry
            {
                Method = method,
                Path = path,
                QueryString = queryString,
                Headers = headers,
                RequestBody = sanitizedBody,
                StatusCode = statusCode,
                ResponseBody = responseBody,
                ElapsedMilliseconds = stopwatch.ElapsedMilliseconds
            };

            var formattedLog = JsonSerializer.Serialize(logentry, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            _logger.LogInformation("Http Log entry: \n{@HttpLogEntry}\n\n", formattedLog);

            await responseBodyStream.CopyToAsync(originalStream);
        }
        private string SanitizeRequestBody(string path, string requestBody)
        {
            if(path.Equals("/api/Auth/login", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    var regex = new Regex(@"""password""\s*:\s*""[^""]*""", RegexOptions.IgnoreCase);
                    return regex.Replace(requestBody, @"""password"": ""[REDACTED]""");
                }
                catch 
                {
                    return "[REDACTED]";
                }
            }
            return requestBody;
        }
    }
}
