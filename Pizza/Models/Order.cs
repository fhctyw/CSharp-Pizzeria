namespace Pizza
{
    public class Order
    {
        public Dictionary<string, int> OrderedPizzas { get; set; }
        public Dictionary<CustomPizza, int> CustomPizzas { get; set; }
        public Order()
        {
            OrderedPizzas = new();
            CustomPizzas = new();
        }
    }
}
