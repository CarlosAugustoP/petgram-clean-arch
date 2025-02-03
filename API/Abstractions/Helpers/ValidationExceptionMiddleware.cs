using FluentValidation;
using System.Text.Json;

namespace API.Abstractions.Helpers
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

            var response = new ValidationErrorResponse
            {
                Type = "ValidationError",
                Title = "Validation Failed",
                Status = StatusCodes.Status400BadRequest,
            };

            foreach (var error in ex.Errors)
            {
                if (!response.Errors.ContainsKey(error.PropertyName))
                {
                    response.Errors[error.PropertyName] = new List<string>();
                }
                response.Errors[error.PropertyName].Add(error.ErrorMessage);
            }

            var jsonResponse = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
}