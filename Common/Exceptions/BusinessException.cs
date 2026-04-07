using Microsoft.AspNetCore.Http;

namespace Common.Exceptions
{
    /// <summary>
    /// Thrown when a business operation fails
    /// </summary>
    public class BusinessException : AppException
    {
        public override int StatusCode => StatusCodes.Status400BadRequest;

        public BusinessException(string message, string? errorCode = "BUSINESS_ERROR")
            : base(message, errorCode)
        {
        }
    }
}
