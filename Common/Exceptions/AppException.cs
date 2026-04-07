namespace Common.Exceptions
{
    /// <summary>
    /// Base custom exception for the application
    /// </summary>
    public abstract class AppException : Exception
    {
        public abstract int StatusCode { get; }
        public string? ErrorCode { get; protected set; }
        public Dictionary<string, string[]>? Errors { get; protected set; }

        protected AppException(string message, string? errorCode = null) : base(message)
        {
            ErrorCode = errorCode;
        }

        protected AppException(string message, string? errorCode, Dictionary<string, string[]>? errors) : base(message)
        {
            ErrorCode = errorCode;
            Errors = errors;
        }
    }
}
