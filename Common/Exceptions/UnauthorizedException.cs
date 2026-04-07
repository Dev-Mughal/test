using Microsoft.AspNetCore.Http;

namespace Common.Exceptions
{
    /// <summary>
    /// Thrown when an operation is unauthorized
    /// </summary>
    public class UnauthorizedException : AppException
    {
        public override int StatusCode => StatusCodes.Status401Unauthorized;

        public UnauthorizedException(string message = "Unauthorized access.", string? errorCode = "UNAUTHORIZED")
            : base(message, errorCode)
        {
        }
    }
}
