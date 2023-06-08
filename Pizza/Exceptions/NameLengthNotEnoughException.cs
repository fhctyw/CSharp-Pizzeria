using System.Runtime.Serialization;

namespace Pizza.Exceptions
{
    public class NameLengthNotEnoughException : Exception
    {
        public NameLengthNotEnoughException()
        {
        }

        public NameLengthNotEnoughException(string? message) : base(message)
        {
        }

        public NameLengthNotEnoughException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NameLengthNotEnoughException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
