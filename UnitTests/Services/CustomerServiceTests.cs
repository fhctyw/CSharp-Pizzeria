using Moq;
using Pizza;

namespace UnitTests.Services
{
    public class CustomerServiceTests
    {
        private Mock<ISerializer> mockSerializer;
        private Mock<IPizzeriaService> mockPizzeriaService;
        private CustomerService customerService;

        [SetUp]
        public void SetUp()
        {
            mockSerializer = new Mock<ISerializer>();
            mockPizzeriaService = new Mock<IPizzeriaService>();

            customerService = new CustomerService(mockSerializer.Object, mockPizzeriaService.Object);
        }

        [Test]
        public void LoadCustomers_ThrowsException_WhenFileNameIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => customerService.LoadCustomers(null));
        }

        [Test]
        public void SaveCustomers_ThrowsException_WhenFileNameIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => customerService.SaveCustomers(null));
        }

        [Test]
        public void GetCustomer_ThrowsCustomerNotFoundException_WhenCustomerDoesNotExist()
        {
            Assert.Throws<CustomerNotFoundException>(() => customerService.GetCustomer("Nonexistent Customer"));
        }

        [Test]
        public void AddCustomer_ThrowsCustomerExistsException_WhenCustomerAlreadyExists()
        {
            var customer = new Customer { Name = "John", Address = "123 Street", Money = 50 };
            customerService.AddCustomer(customer);

            Assert.Throws<CustomerExistsException>(() => customerService.AddCustomer(customer));
        }

        [Test]
        public void AddCustomer_ThrowsArgumentNullException_WhenCustomerNameIsNullOrEmpty()
        {
            var customer = new Customer { Name = "", Address = "123 Street", Money = 50 };
            Assert.Throws<ArgumentNullException>(() => customerService.AddCustomer(customer));
        }

        [Test]
        public void AddCustomer_ThrowsArgumentNullException_WhenCustomerAddressIsNullOrEmpty()
        {
            var customer = new Customer { Name = "John", Address = "", Money = 50 };
            Assert.Throws<ArgumentNullException>(() => customerService.AddCustomer(customer));
        }

        [Test]
        public void AddCustomer_ThrowsArgumentException_WhenCustomerMoneyIsZeroOrLess()
        {
            var customer = new Customer { Name = "John", Address = "123 Street", Money = 0 };
            Assert.Throws<ArgumentException>(() => customerService.AddCustomer(customer));
        }

        [Test]
        public void AddCustomer_AddsCustomerSuccessfully()
        {
            var customer = new Customer { Name = "John", Address = "123 Street", Money = 50 };
            customerService.AddCustomer(customer);

            var retrievedCustomer = customerService.GetCustomer("John");

            Assert.IsNotNull(retrievedCustomer);
            Assert.That(retrievedCustomer.Name, Is.EqualTo("John"));
        }

        [Test]
        public void RemoveCustomer_RemovesCustomerSuccessfully()
        {
            var customer = new Customer { Name = "John", Address = "123 Street", Money = 50 };
            customerService.AddCustomer(customer);

            customerService.RemoveCustomer("John");

            Assert.Throws<CustomerNotFoundException>(() => customerService.GetCustomer("John"));
        }

        [Test]
        public void ChangeCustomer_ChangesCustomerSuccessfully()
        {
            var customer = new Customer { Name = "John", Address = "Old Address", Money = 100 };
            customerService.AddCustomer(customer);

            var newCustomerInfo = new Customer { Name = "John", Address = "New Address", Money = 200 };
            customerService.ChangeCustomer("John", newCustomerInfo);

            var updatedCustomer = customerService.GetCustomer("John");

            Assert.That(updatedCustomer.Address, Is.EqualTo("New Address"));
            Assert.That(updatedCustomer.Money, Is.EqualTo(200));
        }
    }
}
