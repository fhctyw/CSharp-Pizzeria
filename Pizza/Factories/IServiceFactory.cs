namespace Pizza
{
    public interface IServiceFactory
    {
        ICustomerService CreateCustomerService();
        IPizzeriaService CreatePizzeriaService();
    }
}
