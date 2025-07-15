using Microsoft.AspNetCore.Mvc;
using WebAPI.Exceptions;
using WebAPI.Extensions;

namespace WebAPI.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> l)
        {
            _next = next;
            _logger = l;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (MyPosApiException ex)
            {
                await context.Response.WriteProblemDetailsAsync(ex.StatusCode, "Request failed", ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                await context.Response.WriteProblemDetailsAsync(StatusCodes.Status401Unauthorized,
                    "Unauthorized", ex.Message);
            }
            catch (Exception ex)
            {
                // Additional LogError for Unhandled exceptions. Other exceptions just use the standart middleware
                _logger.LogError(ex, "Unhandled exception");

                await context.Response.WriteProblemDetailsAsync(StatusCodes.Status500InternalServerError,
                    "Internal Server Error", ex.Message);
            }
        }
    }
}
