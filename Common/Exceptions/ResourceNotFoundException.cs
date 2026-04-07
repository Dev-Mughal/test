using Microsoft.AspNetCore.Http;

namespace Common.Exceptions
{
    /// <summary>
    /// Thrown when a requested resource is not found
    /// </summary>
    public class ResourceNotFoundException : AppException
    {
        public override int StatusCode => StatusCodes.Status404NotFound;

        public ResourceNotFoundException(string message, string? errorCode = "RESOURCE_NOT_FOUND")
            : base(message, errorCode)
        {
        }
    }
}
