using Microsoft.AspNetCore.Http;

namespace Common.Exceptions
{
    /// <summary>
    /// Thrown when a resource already exists
    /// </summary>
    public class DuplicateResourceException : AppException
    {
        public override int StatusCode => StatusCodes.Status409Conflict;

        public DuplicateResourceException(string message, string? errorCode = "DUPLICATE_RESOURCE")
            : base(message, errorCode)
        {
        }

        public DuplicateResourceException(string resourceName, string fieldName, string value)
            : base($"{resourceName} with {fieldName} '{value}' already exists.", "DUPLICATE_RESOURCE")
        {
        }
    }
}
