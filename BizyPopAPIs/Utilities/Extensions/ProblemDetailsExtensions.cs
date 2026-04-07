using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Utilities.Extensions
{
    public static class ProblemDetailsExtensions
    {
        public static ProblemDetails AddExtensions(
            this ProblemDetails details,
            HttpContext httpContext,
            string errorCode,
            Exception exception)
        {
            details.Extensions["traceId"] = httpContext.TraceIdentifier;
            details.Extensions["errorCode"] = errorCode;

            if (httpContext.RequestServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment())
            {
                details.Extensions["stackTrace"] = exception.StackTrace;
                details.Extensions["exceptionType"] = exception.GetType().Name;
            }

            return details;
        }
    }
}
