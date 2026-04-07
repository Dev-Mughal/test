namespace Common.CustomExceptions
{
    public class UserNotFoundCustomException : Exception
    {
        public UserNotFoundCustomException() : base("The user was not found!")
        {
        }
        public UserNotFoundCustomException(string message) : base(message)
        {
        }
        public UserNotFoundCustomException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
