namespace Pizza
{
    [Serializable]
    public class CustomPizza : StandardPizza, IEquatable<CustomPizza?>
    {
        public Dictionary<string, int> AdditionalIngredients { get; set; }
        public CustomPizza()
        {
            AdditionalIngredients = new();
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as CustomPizza);
        }

        public bool Equals(CustomPizza? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   EqualityComparer<Dictionary<string, int>>.Default.Equals(AdditionalIngredients, other.AdditionalIngredients);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), AdditionalIngredients);
        }

        public static bool operator ==(CustomPizza? left, CustomPizza? right)
        {
            return EqualityComparer<CustomPizza>.Default.Equals(left, right);
        }

        public static bool operator !=(CustomPizza? left, CustomPizza? right)
        {
            return !(left == right);
        }
    }
}
