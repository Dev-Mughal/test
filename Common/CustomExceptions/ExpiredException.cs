
namespace Common.CustomExceptions
{
    public class ExpiredException : Exception
    {
        public ExpiredException() : base("The entity has expired.") { }
        public ExpiredException(string message) : base(message) { }
        public ExpiredException(string message, Exception innerException) : base(message, innerException) { }
    }
}
