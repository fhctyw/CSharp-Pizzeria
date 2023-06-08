namespace Pizza
{
    [Serializable]
    public class Ingredient : IEquatable<Ingredient?>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Ingredient()
        {
            Name = string.Empty;
        }
        public override bool Equals(object? obj)
        {
            return Equals(obj as Ingredient);
        }

        public bool Equals(Ingredient? other)
        {
            return other is not null &&
                   Name == other.Name &&
                   Price == other.Price;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Price);
        }

        public static bool operator ==(Ingredient? left, Ingredient? right)
        {
            return EqualityComparer<Ingredient>.Default.Equals(left, right);
        }

        public static bool operator !=(Ingredient? left, Ingredient? right)
        {
            return !(left == right);
        }
    }
}
