using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using WorkMosm.Domain.CustomExceptions;

namespace WorkMosm.Api.Middlewares
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
                var correlationId = context.Request.Headers.TryGetValue("X-Correlation-Id", out var cid) ? cid.ToString()
                                                                                                         : Guid.NewGuid().ToString();


                var traceId = Activity.Current?.Id ?? context.TraceIdentifier;
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";
                var logLevel = ResolveLogLevel(ex);

                _logger.Log(
                    logLevel,
                    ex,
                    "[CorrelationId: {CorrelationId}] | [TraceId: {TraceId}] Exception | User: {UserId} | Path: {Path} | Method: {Method}",
                    correlationId, traceId, userId, context.Request.Path, context.Request.Method
                );

                await HandleExceptionAsync(context, ex, correlationId, traceId);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, string correlationId, string traceId)
        {
            context.Response.ContentType = "application/problem+json";
            context.Response.Headers["X-Correlation-Id"] = correlationId;

            var (statusCode, title, type) = MapException(exception);

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Type = type,
                Detail = statusCode == 500
                        ? "An unexpected error occurred. Please contact support with the provided traceId."
                        : exception.Message,
                Instance = $"{context.Request.Method} {context.Request.Path}"
            };

            problemDetails.Extensions.Add("traceId", traceId);
            problemDetails.Extensions.Add("correlationId", correlationId);

            EnrichWithValidationErrors(exception, problemDetails);

            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsJsonAsync(problemDetails);
        }

        private static (int, string, string) MapException(Exception ex) => ex switch
        {
            ValidationException => (400, ex.Message, "validation_error"),
            UserAlreadyExistsException => (409, ex.Message, "user_already_exists"),
            UnauthorizedAccessException => (403, ex.Message, "forbidden"),
            KeyNotFoundException => (404, "Resource not found.", "not_found"),
            UserNotFoundException => (404, ex.Message, "not_found"),
            InvalidCredentialsException => (401, ex.Message, "invalid_credentials"),
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

        private static LogLevel ResolveLogLevel(Exception ex) => ex switch
        {
            ValidationException => LogLevel.Information,
            InvalidCredentialsException => LogLevel.Warning,
            UserAlreadyExistsException => LogLevel.Warning,
            UnauthorizedAccessException => LogLevel.Warning,
            _ => LogLevel.Error
        };
    }
}
