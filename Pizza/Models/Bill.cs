namespace Pizza
{
    public class Bill
    {
        public Customer Customer { get; set; }
        public DateTime Time { get; set; }
        public decimal TotalPrice { get; set; }
        public List<StandardPizza> Pizzas { get; set; }

        public Bill()
        {
            Customer = new Customer();
            Pizzas = new List<StandardPizza>();
        }
    }
}
