using System.Runtime.Serialization;

namespace Pizza
{
    public class OrderedPizzaNotFoundException : Exception
    {
        public OrderedPizzaNotFoundException()
        {
        }

        public OrderedPizzaNotFoundException(string? message) : base(message)
        {
        }

        public OrderedPizzaNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected OrderedPizzaNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
