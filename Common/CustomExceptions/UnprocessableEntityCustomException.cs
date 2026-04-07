using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Common.CustomExceptions
{
    /// <summary>
    /// Exception used to represent validation failures that should map to an HTTP 422 Unprocessable Entity.
    /// The exception can be created with a message and a dictionary of validation errors (field -> error messages).
    /// </summary>
    public class UnprocessableEntityCustomException : Exception
    {
        public IDictionary<string, string[]> Errors { get; set; }

        private UnprocessableEntityCustomException(string message, IDictionary<string, string[]> errors)
      : base(message) => Errors = errors;

        /// <summary>
        /// Inspect a dictionary of validation errors and throw an <see cref="UnprocessableEntityCustomException"/>
        /// when the dictionary contains one or more entries.
        /// </summary>
        /// <param name="errors">
        /// A dictionary mapping field names (or keys) to an array of error messages.
        /// Example: { "Email": new[] { "Email is required.", "Email is invalid." } }
        /// If <paramref name="errors"/> is null or empty, this method does nothing.
        /// </param>
        /// <param name="message">
        /// Optional exception message. Defaults to "Validation Failed." This message will be passed into the thrown exception.
        /// </param>
        /// <exception cref="UnprocessableEntityCustomException">
        /// Thrown when <paramref name="errors"/> contains one or more entries. The thrown exception will contain the provided errors.
        /// </exception>
        public static void ThrowIfInvalid(Dictionary<string, string[]> errors, string message = "Validation Failed.")
        {
            if (errors != null && errors.Count > 0)
            {
                throw new UnprocessableEntityCustomException(message, errors);
            }
        }

        /// <summary>
        /// Inspect an ASP.NET Core <see cref="ModelStateDictionary"/> and throw an <see cref="UnprocessableEntityCustomException"/>
        /// when any model state entries contain validation errors.
        /// </summary>
        /// <param name="modelState">
        /// The <see cref="ModelStateDictionary"/> to inspect (for example, use the controller or Razor PageModel's ModelState).
        /// The method will collect all keys that have one or more errors and convert them into a dictionary of messages.
        /// </param>
        /// <param name="message">
        /// Optional exception message. Defaults to "Validation Failed." This message will be passed into the thrown exception.
        /// </param>
        /// <remarks>
        /// Typical usage:
        /// - In a Razor Page handler or controller action, call this method after model binding to centralize 422 error throwing.
        /// - The resulting exception contains a dictionary of field -> error messages which can be serialized to return
        ///   structured validation information to API clients.
        /// </remarks>
        /// <exception cref="UnprocessableEntityCustomException">
        /// Thrown when one or more model state entries contain validation errors.
        /// </exception>
        public static void ThrowIfModelInvalid(ModelStateDictionary modelState, string message = "Please provide all information")
        {
            var errors = modelState
                .Where(e => e.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(er => er.ErrorMessage).ToArray()
                );

            if (errors.Count > 0)
            {
                throw new UnprocessableEntityCustomException(message, errors);
            }
        }
    }
}
