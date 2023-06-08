using System.Runtime.Serialization;

namespace Pizza
{
    public class IngredientNotEnoughException : Exception
    {
        public IngredientNotEnoughException()
        {
        }

        public IngredientNotEnoughException(string? message) : base(message)
        {
        }

        public IngredientNotEnoughException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected IngredientNotEnoughException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
