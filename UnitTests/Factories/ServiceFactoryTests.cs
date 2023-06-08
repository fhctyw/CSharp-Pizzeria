using Moq;
using Pizza;

namespace UnitTests.Factories
{
    internal class ServiceFactoryTests
    {
        private Mock<ISerializer> mockSerializer;

        private ServiceFactory serviceFactory;

        [SetUp]
        public void SetUp()
        {
            mockSerializer = new Mock<ISerializer>();
            
            serviceFactory = new ServiceFactory(mockSerializer.Object);
        }

        [Test]
        public void CreateCustomerService_ReturnsCustomerServiceInstance()
        {
            var result = serviceFactory.CreateCustomerService();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ICustomerService>(result);
        }

        [Test]
        public void CreatePizzeriaService_ReturnsPizzeriaServiceInstance()
        {
            var result = serviceFactory.CreatePizzeriaService();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IPizzeriaService>(result);
        }

        [Test]
        public void CreateCustomerService_CreatesPizzeriaServiceIfNotExists()
        {
            var customerService = serviceFactory.CreateCustomerService();

            Assert.IsNotNull(serviceFactory.CreatePizzeriaService());
        }

        [Test]
        public void CreatePizzeriaService_CreatesCustomerServiceIfNotExists()
        {
            var pizzeriaService = serviceFactory.CreatePizzeriaService();

            Assert.IsNotNull(serviceFactory.CreateCustomerService());
        }
    }
}