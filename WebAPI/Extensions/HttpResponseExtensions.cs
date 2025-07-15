using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace WebAPI.Extensions
{
    public static class HttpResponseExtensions
    {
        public static async Task WriteProblemDetailsAsync(this HttpResponse response,
           int statusCode, string title, string detail)
        {
            response.StatusCode = statusCode;
            response.ContentType = "application/json";

            var problem = new ProblemDetails
            {
                Title = title,
                Status = statusCode,
                Detail = detail
            };
            await response.WriteAsJsonAsync(problem);
        }
    }
}
