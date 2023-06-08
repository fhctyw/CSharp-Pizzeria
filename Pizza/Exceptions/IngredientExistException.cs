using System.Runtime.Serialization;

namespace Pizza
{
    public class IngredientExistException : Exception
    {
        public IngredientExistException()
        {
        }

        public IngredientExistException(string? message) : base(message)
        {
        }

        public IngredientExistException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected IngredientExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
