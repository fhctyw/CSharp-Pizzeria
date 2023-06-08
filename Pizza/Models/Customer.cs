namespace Pizza
{
    [Serializable]
    public class Customer
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal Money { get; set; }
        public Customer()
        {
            Name = string.Empty;
            Address = string.Empty;
        }
    }
}
