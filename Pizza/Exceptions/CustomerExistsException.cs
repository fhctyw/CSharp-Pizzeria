using System.Runtime.Serialization;

namespace Pizza
{
    public class CustomerExistsException : Exception
    {
        public CustomerExistsException()
        {
        }

        public CustomerExistsException(string? message) : base(message)
        {
        }

        public CustomerExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CustomerExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
