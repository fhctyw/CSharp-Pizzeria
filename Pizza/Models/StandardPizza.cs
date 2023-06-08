namespace Pizza
{
    public class StandardPizza : IEquatable<StandardPizza?>
    {
        public string Name { get; set; }
        public Dictionary<string, int> NeededIngredients { get; set; }
        public decimal Price { get; set; }

        public StandardPizza()
        {
            Name = string.Empty;
            NeededIngredients = new();
        }
        public override string ToString()
        {
            return $"Pizza {Name} {Price:0.00}";
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as StandardPizza);
        }

        public bool Equals(StandardPizza? other)
        {
            return other is not null &&
                   Name == other.Name &&
                   EqualityComparer<Dictionary<string, int>>.Default.Equals(NeededIngredients, other.NeededIngredients) &&
                   Price == other.Price;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, NeededIngredients, Price);
        }

        public static bool operator ==(StandardPizza? left, StandardPizza? right)
        {
            return EqualityComparer<StandardPizza>.Default.Equals(left, right);
        }

        public static bool operator !=(StandardPizza? left, StandardPizza? right)
        {
            return !(left == right);
        }
    }
}
