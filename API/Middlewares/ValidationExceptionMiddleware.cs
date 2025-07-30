using API.Abstractions.Helpers;
using API.Abstractions.Result;
using FluentValidation;
using System.Text.Json;

namespace API.Middlewares
{
    public class ValidationExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";

                var response = new ValidationErrorResponse();

                foreach (var error in ex.Errors)
                {
                    if (!response.Errors.ContainsKey(error.PropertyName))
                    {
                        response.Errors[error.PropertyName] = new List<string>();
                    }
                    response.Errors[error.PropertyName].Add(error.ErrorMessage);
                }

                var result = Result<ValidationErrorResponse>.ValidationFailure(response);
                var jsonResponse = JsonSerializer.Serialize(result, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}