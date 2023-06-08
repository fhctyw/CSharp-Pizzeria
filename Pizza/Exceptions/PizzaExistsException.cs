using System.Runtime.Serialization;

namespace Pizza
{
    public class PizzaExistsException : Exception
    {
        public PizzaExistsException()
        {
        }

        public PizzaExistsException(string? message) : base(message)
        {
        }

        public PizzaExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected PizzaExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
