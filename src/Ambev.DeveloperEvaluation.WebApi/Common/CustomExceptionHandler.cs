using System.Net;
using System.Text.Json;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.WebApi.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace Ambev.DeveloperEvaluation.WebApi.Common;

public class CustomExceptionHandler : IExceptionHandler
{
    private readonly IHostEnvironment _env;
    private readonly ILogger<CustomExceptionHandler> _logger;
    private readonly Dictionary<Type, Func<HttpContext, Exception, Task>> _handlers;

    public CustomExceptionHandler(IHostEnvironment env, ILogger<CustomExceptionHandler> logger)
    {
        _env = env;
        _logger = logger;

        _handlers = new Dictionary<Type, Func<HttpContext, Exception, Task>>
        {
            { typeof(ValidationException), HandleValidationException },
            { typeof(KeyNotFoundException), HandleNotFoundException },
            { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
            { typeof(ForbiddenAccessException), HandleForbiddenAccessException },
            { typeof(DomainException), HandleDomainException },
            { typeof(Exception), HandleUnhandledException }
        };
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception ex, CancellationToken ct)
    {
        var type = ex.GetType();

        while (type is not null)
        {
            if (_handlers.TryGetValue(type, out var handler))
            {
                await handler(httpContext, ex);
                return true;
            }

            type = type.BaseType;
        }

        await HandleUnhandledException(httpContext, ex);
        return true;
    }

    private static Task WriteApiAsync(HttpContext ctx, HttpStatusCode status, string message,
        IEnumerable<ValidationErrorDetail>? errors = null)
    {
        ctx.Response.ContentType = "application/json";
        ctx.Response.StatusCode = (int)status;

        var payload = new ApiResponse
        {
            Success = false,
            Message = message,
            Errors = errors ?? []
        };

        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        return ctx.Response.WriteAsync(JsonSerializer.Serialize(payload, jsonOptions));
    }

    private Task HandleValidationException(HttpContext ctx, Exception ex)
    {
        var validationException = (ValidationException)ex;

        var errors = validationException.Errors.Select(e => (ValidationErrorDetail)e);

        return WriteApiAsync(ctx, HttpStatusCode.BadRequest, "Validation Failed", errors);
    }

    private Task HandleNotFoundException(HttpContext ctx, Exception ex)
    {
        return WriteApiAsync(ctx, HttpStatusCode.NotFound, "The specified resource was not found.",
            [new ValidationErrorDetail { Error = "NotFound", Detail = ex.Message }]);
    }

    private Task HandleUnauthorizedAccessException(HttpContext ctx, Exception ex)
    {
        return WriteApiAsync(ctx, HttpStatusCode.Unauthorized, "Unauthorized",
            [new ValidationErrorDetail { Error = "Unauthorized", Detail = ex.Message }]);
    }

    private Task HandleForbiddenAccessException(HttpContext ctx, Exception ex)
    {
        return WriteApiAsync(ctx, HttpStatusCode.Forbidden, "Forbidden",
            [new ValidationErrorDetail { Error = "Forbidden", Detail = ex.Message }]);
    }

    private Task HandleDomainException(HttpContext ctx, Exception ex)
    {
        var domainException = (DomainException)ex;
        return WriteApiAsync(ctx, HttpStatusCode.BadRequest, "Domain error occurred.",
            [new ValidationErrorDetail { Error = "DomainException", Detail = domainException.Message }]);
    }

    private Task HandleUnhandledException(HttpContext ctx, Exception ex)
    {
        _logger.LogError(ex, "Unhandled exception. TraceId={TraceId}", ctx.TraceIdentifier);

        var msg = _env.IsDevelopment()
            ? ex.Message
            : "An unexpected error has occurred.";

        var errors = _env.IsDevelopment()
            ? new[] { new ValidationErrorDetail { Error = ex.GetType().Name, Detail = ex.ToString() } }
            : null;

        ctx.Response.Headers["X-Trace-Id"] = ctx.TraceIdentifier;

        return WriteApiAsync(ctx, HttpStatusCode.InternalServerError, msg, errors);
    }
}