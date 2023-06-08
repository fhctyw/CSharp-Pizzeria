namespace Pizza
{
    public class ServiceFactory : IServiceFactory
    {
        protected ISerializer serializer;
        protected ICustomerService? customerService;
        protected IPizzeriaService? pizzeriaService;
        public ServiceFactory(ISerializer serializer)
        {
            this.serializer = serializer;
        }

        public ICustomerService CreateCustomerService()
        {
            customerService = new CustomerService(serializer, pizzeriaService);
            if (pizzeriaService == null)
            {
                pizzeriaService = new PizzeriaService(serializer, customerService);
            }
            return customerService;
        }

        public IPizzeriaService CreatePizzeriaService()
        {
            pizzeriaService = new PizzeriaService(serializer, customerService);
            if (customerService == null)
            {
                customerService = new CustomerService(serializer, pizzeriaService);
            }
            return pizzeriaService;
        }
    }
}
