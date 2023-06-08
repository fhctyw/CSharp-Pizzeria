namespace Pizza
{
    public class CustomerService : ICustomerService
    {
        protected ISerializer serializer;
        protected IPizzeriaService pizzeriaService;
        protected List<Customer> customers = new List<Customer>();
        public CustomerService(ISerializer serializer, IPizzeriaService pizzeriaService)
        {
            this.serializer = serializer;
            this.pizzeriaService = pizzeriaService;
        }
        public void LoadCustomers(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("File name cannot be null");
            }
            customers = serializer.Deserialize<List<Customer>>(RepositoryHelpers.GetFilePath(fileName));
        }
        public void SaveCustomers(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("File name cannot be null");
            }
            if (!Directory.Exists(RepositoryHelpers.FolderName))
            {
                Directory.CreateDirectory(RepositoryHelpers.FolderName);
            }
            serializer.Serialize(RepositoryHelpers.GetFilePath(fileName), customers);
        }
        public List<Customer> GetCustomers()
        {
            return customers;
        }
        public Customer GetCustomer(string name)
        {
            ServiceHelpers.CheckName(name);
            Customer? foundCustomer = customers.FirstOrDefault(c => c.Name == name);
            if (foundCustomer == default)
            {
                throw new CustomerNotFoundException($"cannot find customer with \"{name}\" name");
            }
            return foundCustomer;
        }

        public void ChangeCustomer(string name, Customer customer)
        {
            ServiceHelpers.CheckName(name);
            ServiceHelpers.CheckCustomer(customer);

            Customer foundCustomer = GetCustomer(name);
            foundCustomer.Address = customer.Address;
            foundCustomer.Money = customer.Money;
        }

        public void RemoveCustomer(string name)
        {
            ServiceHelpers.CheckName(name);
            Customer? foundCustomer = customers.FirstOrDefault(c => c.Name == name);
            if (foundCustomer == default)
            {
                throw new CustomerNotFoundException($"cannot find customer with \"{name}\" name");
            }
            customers = customers.Where(c => c.Name != name).ToList();
        }

        public void AddCustomer(Customer customer)
        {
            ServiceHelpers.CheckCustomer(customer);
            Customer? foundCustomer = customers.FirstOrDefault(c => c.Name == customer.Name);
            if (foundCustomer != default)
            {
                throw new CustomerExistsException($"customer with \"{customer.Name}\" already exists");
            }
            customers.Add(customer);
        }
    }
}
