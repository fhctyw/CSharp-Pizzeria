using System.Runtime.Serialization;

namespace Pizza
{
    public class WrongPriceException : Exception
    {
        public WrongPriceException()
        {
        }

        public WrongPriceException(string? message) : base(message)
        {
        }

        public WrongPriceException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected WrongPriceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
