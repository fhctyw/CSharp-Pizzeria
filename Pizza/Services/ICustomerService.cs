namespace Pizza
{
    public interface ICustomerService
    {
        void LoadCustomers(string fileName);
        void SaveCustomers(string fileName);
        List<Customer> GetCustomers();
        Customer GetCustomer(string name);
        void AddCustomer(Customer customer);
        void ChangeCustomer(string name, Customer customer);
        void RemoveCustomer(string name);
    }
}
