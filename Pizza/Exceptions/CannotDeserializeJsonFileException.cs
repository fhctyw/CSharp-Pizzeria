using System.Runtime.Serialization;

namespace Pizza
{
    public class CannotDeserializeJsonFileException : Exception
    {
        public CannotDeserializeJsonFileException()
        {
        }

        public CannotDeserializeJsonFileException(string? message) : base(message)
        {
        }

        public CannotDeserializeJsonFileException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CannotDeserializeJsonFileException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
