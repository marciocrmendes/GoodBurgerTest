using FluentValidation;
using GoodBurger.Api.Infrastructure.Extensions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GoodBurger.Api.Infrastructure.Configurations;

internal sealed class ExceptionHandler(ILogger<ExceptionHandler> logger,
    IWebHostEnvironment environment
) : IExceptionHandler
{
    private readonly ILogger<ExceptionHandler> _logger = logger;
    private readonly IWebHostEnvironment _environment = environment;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken stoppingToken
    )
    {
        if (httpContext.Response.HasStarted)
        {
            _logger.LogWarning("Cannot write error response, response has already started");
            return false;
        }

        return exception switch
        {
            ValidationException validationException =>
                await HandleValidationExceptionAsync(httpContext, validationException, stoppingToken),

            _ =>
                await HandleGenericExceptionAsync(httpContext, exception, stoppingToken)
        };
    }

    private async Task<bool> HandleValidationExceptionAsync(
        HttpContext httpContext,
        ValidationException validationException,
        CancellationToken stoppingToken)
    {
        _logger.LogWarning(validationException, "Validation error occurred");

        var statusCode = (int)HttpStatusCode.BadRequest;
        var validationProblemDetails = new ValidationProblemDetails()
        {
            Title = "Validation Error",
            Detail = "One or more validation errors occurred",
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
            Status = statusCode,
            Extensions = new Dictionary<string, object?>()
            {
                ["traceId"] = httpContext.TraceIdentifier
            }
        };

        validationProblemDetails.AddValidationErrors(validationException.Errors);
        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(validationProblemDetails, stoppingToken);
        return true;
    }

    private async Task<bool> HandleGenericExceptionAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken stoppingToken)
    {
        _logger.LogError(exception, "Unhandled exception occurred: {Message}", exception.Message);

        var statusCode = (int)HttpStatusCode.InternalServerError;
        var problemDetails = new ProblemDetails
        {
            Title = "Server Error",
            Detail = "An unexpected error occurred. Please try again later",
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
            Status = statusCode,
            Extensions = new Dictionary<string, object?>()
            {
                ["traceId"] = httpContext.TraceIdentifier
            }
        };

        if (!_environment.IsProduction())
        {
            problemDetails.Extensions["exception"] = exception.Message;
            problemDetails.Extensions["stackTrace"] = exception.StackTrace;
            problemDetails.Extensions["type"] = exception.GetType().Name;

            if (exception.InnerException != null)
            {
                problemDetails.Extensions["innerException"] = exception.InnerException.Message;
            }
        }

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, stoppingToken);
        return true;
    }
}