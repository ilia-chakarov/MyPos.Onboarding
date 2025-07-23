using Microsoft.AspNetCore.Http;
using MyPos.Services.DTOs.Log;
using System.Diagnostics;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using WebAPI.Exceptions;
using WebAPI.Extensions;

namespace WebAPI.Middleware
{
    public class HttpAndExceptionLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HttpAndExceptionLoggingMiddleware> _logger;

        public HttpAndExceptionLoggingMiddleware(RequestDelegate next, ILogger<HttpAndExceptionLoggingMiddleware> l)
        {
            _next = next;
            _logger = l;
        }

        public async Task Invoke(HttpContext context)
        {
            // Prepare for logging
            var stopwatch = Stopwatch.StartNew();

            var request = context.Request;
            var method = request.Method;
            var path = request.Path;

            var queryString = request.QueryString.HasValue ? request.QueryString.Value : string.Empty;
            var headers = request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString());

            foreach (var h in headers)
            {
                if (h.Key.Equals("Authorization"))
                {
                    string sanitized = SanitizeRequestHeader(h.Value);
                    headers[h.Key] = sanitized;
                }
                    
                
            }

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

            // Handle exceptions
            Exception? caughtException = null;
            try
            {
                await _next(context);
            }
            catch (MyPosApiException ex)
            {
                caughtException = ex;
                await context.Response.WriteProblemDetailsAsync(ex.StatusCode, "Request failed", ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                caughtException = ex;
                await context.Response.WriteProblemDetailsAsync(StatusCodes.Status401Unauthorized,
                    "Unauthorized", ex.Message);
            }
            catch (Exception ex)
            {
                caughtException = ex;

                await context.Response.WriteProblemDetailsAsync(StatusCodes.Status500InternalServerError,
                    "Internal Server Error", ex.Message);
            }
            finally
            {
                // log
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
                    ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                    ExceptionMessage = caughtException?.Message,
                    ExceptionStackTrace = caughtException?.StackTrace
                };

                var formattedLog = JsonSerializer.Serialize(logentry, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                _logger.LogInformation("Http Log entry: \n{@HttpLogEntry}\n\n", formattedLog);

                await responseBodyStream.CopyToAsync(originalStream);
            }
        }

        private string SanitizeRequestHeader(string requestHeader)
        {
            
            try
            {
                if (requestHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
                {
                    return "Basic [REDACTED]";
                }
                if (requestHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    return "Bearer [REDACTED]";
                }
                return requestHeader;
            }
            catch
            {
                return "[REDACTED]";
            }
            
            return requestHeader;
        }
        private string SanitizeRequestBody(string path, string requestBody)
        {
            if (path.Equals("/api/Auth/login", StringComparison.OrdinalIgnoreCase))
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
