using Microsoft.AspNetCore.Http;

namespace Common.Exceptions
{
    /// <summary>
    /// Thrown when an operation is forbidden
    /// </summary>
    public class ForbiddenException : AppException
    {
        public override int StatusCode => StatusCodes.Status403Forbidden;

        public ForbiddenException(string message = "Access denied.", string? errorCode = "FORBIDDEN")
            : base(message, errorCode)
        {
        }
    }
}
