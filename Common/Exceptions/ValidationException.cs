using Microsoft.AspNetCore.Http;

namespace Common.Exceptions
{
    /// <summary>
    /// Thrown when validation fails
    /// </summary>
    public class ValidationException : AppException
    {
        public override int StatusCode => StatusCodes.Status422UnprocessableEntity;

        public ValidationException(string message, Dictionary<string, string[]>? errors = null)
            : base(message, "VALIDATION_FAILED", errors)
        {
        }

        public ValidationException(Dictionary<string, string[]> errors)
            : base("One or more validation errors occurred.", "VALIDATION_FAILED", errors)
        {
        }
    }
}
