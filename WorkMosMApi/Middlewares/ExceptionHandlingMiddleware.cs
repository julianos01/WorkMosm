using Domain.CustomExceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace WorkMosmApi.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var traceId = Activity.Current?.Id ?? context.TraceIdentifier;
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";
                _logger.LogError(ex, "[{TraceId}] Unhandled Exception | User: {UserId} | Path: {Path} | Method: {Method}",
                traceId, userId, context.Request.Path, context.Request.Method);

                await HandleExceptionAsync(context, ex, traceId);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, string traceId)
        {
            context.Response.ContentType = "application/problem+json";

            var (statusCode, title, type) = MapException(exception);

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Type = type,
                Detail = exception.Message,
                Instance = $"{context.Request.Method} {context.Request.Path}"
            };

            problemDetails.Extensions.Add("traceId", traceId);

            EnrichWithValidationErrors(exception, problemDetails);

            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsJsonAsync(problemDetails);
        }

        private static (int, string, string) MapException(Exception ex) => ex switch
        {
            ValidationException => (400, "One or more validation errors occurred.", "validation_error"),
            UnauthorizedAccessException => (403, ex.Message, "forbidden"),
            KeyNotFoundException => (404, "Resource not found.", "not_found"),
            UserNotFoundException => (404, ex.Message, "not_found"),
            InvalidCredentialsException => (401, "Invalid email or password.", "invalid_credentials"),
            _ => (500, "An unexpected error occurred.", "internal_server_error")
        };

        private static IDictionary<string, string[]>? EnrichWithValidationErrors(Exception ex, ProblemDetails problemDetails)
        {
            if (ex is ValidationException fluentEx)
            {
                var errors = fluentEx.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

                problemDetails.Extensions.Add("errors", errors);
            }
            return null;
        }
    }
}
