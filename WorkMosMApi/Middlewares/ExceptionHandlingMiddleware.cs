using Domain.CustomExceptions;
using System.Security.Claims;
using WorkMosmApi.Models.Errors;

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
                var traceId = context.TraceIdentifier;
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";
                _logger.LogError(ex, "[{TraceId}] Unhandled Exception | User: {UserId} | Path: {Path} | Method: {Method}",
                traceId, userId, context.Request.Path, context.Request.Method);
                await HandleExceptionAsync(context, ex, traceId);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, string traceId)
        {
            context.Response.ContentType = "application/json";

            var (statusCode, message, type) = exception switch
            {
                UnauthorizedAccessException => (
                    StatusCodes.Status403Forbidden,
                    exception.Message,
                    "forbidden"),

                KeyNotFoundException => (
                    StatusCodes.Status404NotFound,
                    "The requested resource was not found.",
                    "not_found"),

                InvalidCredentialsException => (
                    StatusCodes.Status401Unauthorized,
                    "Invalid email or password.",
                    "invalid_credentials"),

                _ => (
                    StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred on the server.",
                    "internal_server_error")
            };

            var response = new ErrorResponse
            {
                StatusCode = statusCode,
                Message = message,
                Type = type,
                TraceId = traceId
            };

            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
