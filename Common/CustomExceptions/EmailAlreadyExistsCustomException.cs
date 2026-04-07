namespace Common.CustomExceptions
{
    public class EmailAlreadyExistsCustomException : Exception
    {
        public EmailAlreadyExistsCustomException() : base("The specified email already exists.")
        {
        }
        public EmailAlreadyExistsCustomException(string message) : base(message)
        {
        }
        public EmailAlreadyExistsCustomException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
