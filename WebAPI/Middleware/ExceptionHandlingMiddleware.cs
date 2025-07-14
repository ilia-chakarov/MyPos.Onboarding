using Microsoft.AspNetCore.Mvc;
using WebAPI.Exceptions;

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
            }catch(MyPosApiException ex)
            {
                context.Response.StatusCode = ex.StatusCode;
                context.Response.ContentType = "application/json";
                var problem = new ProblemDetails
                {
                    Title = "Request failed",
                    Status = ex.StatusCode,
                    Detail = ex.Message
                };
                await context.Response.WriteAsJsonAsync(problem);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var problem = new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Status = 500,
                    Detail = "An unexpected error occurred"
                };
                await context.Response.WriteAsJsonAsync(problem);
            }
        }
    }
}
