namespace BizyPopAPIs.Utilities.CustomMiddlewares
{
    using Common.Exceptions;
    using Common.Logging;
    using Common.Models;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Mvc;
    using BizyPopAPIs.Utilities.Extensions;
    using System.Text.Json;

    namespace Application.Middleware
    {
        public class GlobalExceptionHandler : IExceptionHandler
        {
            private readonly ILogger<GlobalExceptionHandler> _logger;
            private readonly IAppLogger _appLogger;
            private readonly IWebHostEnvironment _env;

            public GlobalExceptionHandler(
                ILogger<GlobalExceptionHandler> logger,
                IAppLogger appLogger,
                IWebHostEnvironment env)
            {
                _logger = logger;
                _appLogger = appLogger;
                _env = env;
            }

            public async ValueTask<bool> TryHandleAsync(
                HttpContext httpContext,
                Exception exception,
                CancellationToken cancellationToken)
            {
                _logger.LogError(exception, "Exception occurred: {ExceptionMessage}", exception.Message);

                var response = exception switch
                {
                    AppException appEx => HandleAppException(httpContext, appEx),
                    ArgumentNullException argNullEx => HandleArgumentNullException(httpContext, argNullEx),
                    ArgumentException argEx => HandleArgumentException(httpContext, argEx),
                    System.InvalidOperationException opEx => HandleInvalidOperationException(httpContext, opEx),
                    _ => HandleGenericException(httpContext, exception)
                };

                await _appLogger.LogAsync(new LogEntryRequest(
                    Level: AppLogLevel.Error,
                    Message: exception.Message,
                    Category: nameof(GlobalExceptionHandler),
                    EventName: "UnhandledException",
                    Data: new
                    {
                        TraceId = httpContext.TraceIdentifier,
                        StatusCode = response.Status,
                        Request = httpContext.Request.Path.Value,
                        QueryString = httpContext.Request.QueryString.Value
                    },
                    Exception: exception,
                    CorrelationId: httpContext.TraceIdentifier,
                    RequestPath: httpContext.Request.Path,
                    HttpMethod: httpContext.Request.Method
                ), cancellationToken);

                httpContext.Response.StatusCode = response.Status ?? StatusCodes.Status500InternalServerError;
                httpContext.Response.ContentType = "application/problem+json";

                await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
                return true;
            }

            private ProblemDetails HandleAppException(HttpContext httpContext, AppException exception)
            {
                var statusCode = exception.StatusCode;
                
                _logger.LogWarning("Application exception handled: {ErrorCode} - {Message}", 
                    exception.ErrorCode, exception.Message);

                var details = new ProblemDetails
                {
                    Status = statusCode,
                    Title = GetTitleFromStatusCode(statusCode),
                    Detail = exception.Message,
                    Instance = httpContext.Request.Path,
                    Type = $"https://httpstatuses.com/{statusCode}"
                };

                details.Extensions["traceId"] = httpContext.TraceIdentifier;
                details.Extensions["errorCode"] = exception.ErrorCode ?? "APP_ERROR";
                
                if (exception.Errors is not null && exception.Errors.Count > 0)
                {
                    details.Extensions["errors"] = exception.Errors;
                }

                if (_env.IsDevelopment())
                {
                    details.Extensions["stackTrace"] = exception.StackTrace;
                }

                return details;
            }

            private ProblemDetails HandleArgumentNullException(HttpContext httpContext, ArgumentNullException exception)
            {
                _logger.LogError("Argument null exception: {ParamName}", exception.ParamName);

                return new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Bad Request",
                    Detail = $"Required parameter '{exception.ParamName}' is null or empty.",
                    Instance = httpContext.Request.Path,
                    Type = "https://httpstatuses.com/400"
                }.AddExtensions(httpContext, "ARGUMENT_NULL", exception);
            }

            private ProblemDetails HandleArgumentException(HttpContext httpContext, ArgumentException exception)
            {
                _logger.LogError("Argument exception: {Message}", exception.Message);

                return new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Bad Request",
                    Detail = exception.Message,
                    Instance = httpContext.Request.Path,
                    Type = "https://httpstatuses.com/400"
                }.AddExtensions(httpContext, "INVALID_ARGUMENT", exception);
            }

            private ProblemDetails HandleInvalidOperationException(HttpContext httpContext, System.InvalidOperationException exception)
            {
                _logger.LogError("Invalid operation exception: {Message}", exception.Message);

                return new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid Operation",
                    Detail = exception.Message,
                    Instance = httpContext.Request.Path,
                    Type = "https://httpstatuses.com/400"
                }.AddExtensions(httpContext, "INVALID_OPERATION", exception);
            }

            private ProblemDetails HandleGenericException(HttpContext httpContext, Exception exception)
            {
                _logger.LogError("Unhandled exception: {Message}", exception.Message);

                var details = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Internal Server Error",
                    Detail = _env.IsDevelopment() 
                        ? exception.Message 
                        : "An unexpected error occurred. Please try again later.",
                    Instance = httpContext.Request.Path,
                    Type = "https://httpstatuses.com/500"
                };

                details.Extensions["traceId"] = httpContext.TraceIdentifier;

                if (_env.IsDevelopment())
                {
                    details.Extensions["stackTrace"] = exception.StackTrace;
                    details.Extensions["exceptionType"] = exception.GetType().Name;
                }

                return details;
            }

            private static string GetTitleFromStatusCode(int statusCode) => statusCode switch
            {
                400 => "Bad Request",
                401 => "Unauthorized",
                403 => "Forbidden",
                404 => "Not Found",
                409 => "Conflict",
                422 => "Unprocessable Entity",
                500 => "Internal Server Error",
                _ => "Error"
            };
        }
    }
}

