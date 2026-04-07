using Microsoft.AspNetCore.Http;

namespace Common.Exceptions
{
    /// <summary>
    /// Thrown when an invalid operation is attempted
    /// </summary>
    public class InvalidOperationException : AppException
    {
        public override int StatusCode => StatusCodes.Status400BadRequest;

        public InvalidOperationException(string message, string? errorCode = "INVALID_OPERATION")
            : base(message, errorCode)
        {
        }
    }
}
