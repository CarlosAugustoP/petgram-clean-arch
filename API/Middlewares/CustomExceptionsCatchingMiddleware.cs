using Domain.CustomExceptions;
using System.Text.Json;

namespace API.Middlewares
{
    public class CustomExceptionsCatchingMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionsCatchingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ApiException ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)ex.StatusCode;

                var response = Abstractions.Result.Result<string>.Failure(ex.Message);

                var jsonResponse = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(jsonResponse);
            }
            // catch (Exception ex)
            // {
            //     context.Response.ContentType = "application/json";
            //     context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            //     var response = Abstractions.Result.Result<string>.Failure("An unexpected error occurred.");
            //     Console.WriteLine("An unexpected error occurred: " + ex.Message);
            //     var jsonResponse = JsonSerializer.Serialize(response);
            //     await context.Response.WriteAsync(jsonResponse);
            // }
        }
    }
}