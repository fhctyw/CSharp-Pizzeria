namespace Pizza
{
    public class Order
    {
        // TODO or better Dictionary<CustomPizza, int> because CustomPizza is inherited from OrderedPizza?
        public Dictionary<string, int> OrderedPizzas { get; set; }
        public Dictionary<CustomPizza, int> CustomPizzas { get; set; }
        public Order()
        {
            OrderedPizzas = new();
            CustomPizzas = new();
        }
    }
}
