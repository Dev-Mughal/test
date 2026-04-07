namespace Common.CustomExceptions
{
    public class AlreadyExistsCustomException : Exception
    {
        public AlreadyExistsCustomException(string message) : base(message)
        {
        }
        public AlreadyExistsCustomException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
