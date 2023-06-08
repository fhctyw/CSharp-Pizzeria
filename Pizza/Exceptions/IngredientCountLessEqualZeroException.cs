using System.Runtime.Serialization;

namespace Pizza
{
    public class IngredientCountLessEqualZeroException : Exception
    {
        public IngredientCountLessEqualZeroException()
        {
        }

        public IngredientCountLessEqualZeroException(string? message) : base(message)
        {
        }

        public IngredientCountLessEqualZeroException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected IngredientCountLessEqualZeroException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
