using Moq;
using Pizza;

namespace UnitTests.Services
{
    [TestFixture]
    public class PizzeriaServiceTests
    {
        private Mock<ISerializer> mockSerializer;
        private Mock<ICustomerService> mockCustomerService;
        private PizzeriaService pizzeriaService;

        [SetUp]
        public void SetUp()
        {
            mockSerializer = new Mock<ISerializer>();
            mockCustomerService = new Mock<ICustomerService>();

            pizzeriaService = new PizzeriaService(mockSerializer.Object, mockCustomerService.Object);
        }

        [Test]
        public void LoadPizzeria_ThrowsException_WhenFileNameIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => pizzeriaService.LoadPizzeria(null));
        }

        [Test]
        public void SavePizzeria_ThrowsException_WhenFileNameIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => pizzeriaService.SavePizzeria(null));
        }

        [Test]
        public void GetAvailableIngredient_ThrowsIngredientNotFoundException_WhenIngredientDoesNotExist()
        {
            Assert.Throws<IngredientNotFoundException>(() => pizzeriaService.GetAvailableIngredient("Nonexistent Ingredient"));
        }

        [Test]
        public void AddAvailableIngredient_ThrowsException_WhenIngredientAlreadyExists()
        {
            var ingredient = new Ingredient { Name = "Cheese", Price = 1 };
            pizzeriaService.AddAvailableIngredient(ingredient);

            Assert.Throws<IngredientExistException>(() => pizzeriaService.AddAvailableIngredient(ingredient));
        }

        [Test]
        public void AddIngredients_ThrowsException_WhenCountIsZeroOrLess()
        {
            var ingredient = new Ingredient { Name = "Cheese", Price = 1 };
            pizzeriaService.AddAvailableIngredient(ingredient);

            Assert.Throws<IngredientCountLessEqualZeroException>(() => pizzeriaService.AddIngredients("Cheese", 0));
        }

        [Test]
        public void AddIngredients_ThrowsIngredientNotFoundException_WhenIngredientDoesNotExist()
        {
            Assert.Throws<IngredientNotFoundException>(() => pizzeriaService.AddIngredients("Nonexistent Ingredient", 1));
        }

        [Test]
        public void ChangeAvailableIngredient_ThrowsIngredientNotFoundException_WhenIngredientDoesNotExist()
        {
            var ingredient = new Ingredient { Name = "New Cheese", Price = 0.67M };
            Assert.Throws<IngredientNotFoundException>(() => pizzeriaService.ChangeAvailableIngredient("Cheese", ingredient));
        }

        [Test]
        public void RemoveAvailableIngredient_ThrowsIngredientNotFoundException_WhenIngredientDoesNotExist()
        {
            Assert.Throws<IngredientNotFoundException>(() => pizzeriaService.RemoveAvailableIngredient("Nonexistent Ingredient"));
        }

        [Test]
        public void AddAvailableIngredient_AddsIngredientSuccessfully()
        {
            var ingredient = new Ingredient { Name = "Cheese", Price = 1 };
            pizzeriaService.AddAvailableIngredient(ingredient);

            var retrievedIngredient = pizzeriaService.GetAvailableIngredient("Cheese");

            Assert.IsNotNull(retrievedIngredient);
            Assert.That(retrievedIngredient.Name, Is.EqualTo("Cheese"));
        }

        [Test]
        public void RemoveAvailableIngredient_RemovesIngredientSuccessfully()
        {
            var ingredient = new Ingredient { Name = "Cheese", Price = 1 };
            pizzeriaService.AddAvailableIngredient(ingredient);

            pizzeriaService.RemoveAvailableIngredient("Cheese");

            Assert.Throws<IngredientNotFoundException>(() => pizzeriaService.GetAvailableIngredient("Cheese"));
        }
    }
}
