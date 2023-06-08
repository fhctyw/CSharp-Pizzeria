using Moq;
using Pizza;
using Pizza.Exceptions;

namespace UnitTests.Services
{
    [TestFixture]
    public class PizzeriaServiceTests
    {
        private Mock<ISerializer> serializerMock;
        private Mock<ICustomerService> customerServiceMock;
        private PizzeriaService pizzeriaService;

        [SetUp]
        public void Setup()
        {
            serializerMock = new Mock<ISerializer>();
            customerServiceMock = new Mock<ICustomerService>();
            pizzeriaService = new PizzeriaService(serializerMock.Object, customerServiceMock.Object);
        }

        [Test]
        public void LoadPizzeria_FileNameIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => pizzeriaService.LoadPizzeria(null));
        }

        [Test]
        public void LoadPizzeria_FileNameIsNotEmpty_CallsSerializerDeserialize()
        {
            var fileName = "pizzeria.json";

            pizzeriaService.LoadPizzeria(fileName);

            serializerMock.Verify(s => s.Deserialize<Pizzeria>(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void SavePizzeria_FileNameIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => pizzeriaService.SavePizzeria(null));
        }

        [Test]
        public void SavePizzeria_DirectoryDoesNotExist_CreatesDirectory()
        {
            var directory = RepositoryHelpers.FolderName;
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
            }

            var fileName = "pizzeria.json";

            pizzeriaService.SavePizzeria(fileName);

            Assert.True(Directory.Exists(directory));
        }

        [Test]
        public void SavePizzeria_FileNameIsNotEmpty_CallsSerializerSerialize()
        {
            var fileName = "pizzeria.json";

            pizzeriaService.SavePizzeria(fileName);

            serializerMock.Verify(s => s.Serialize(It.IsAny<string>(), It.IsAny<Pizzeria>()), Times.Once);
        }

        [Test]
        public void GetPizzeria_ReturnsPizzeriaInstance()
        {
            var result = pizzeriaService.GetPizzeria();

            Assert.IsInstanceOf<Pizzeria>(result);
        }
        [Test]
        public void GetAvailableIngredient_NameIsEmpty_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => pizzeriaService.GetAvailableIngredient(string.Empty));
        }

        [Test]
        public void GetAvailableIngredient_NameLengthLessEqualTwo_ThrowsNameLengthNotEnoughException()
        {
            Assert.Throws<NameLengthNotEnoughException>(() => pizzeriaService.GetAvailableIngredient("a"));
        }

        
        

        [Test]
        public void AddAvailableIngredient_IngredientIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => pizzeriaService.AddAvailableIngredient(null));
        }

        [Test]
        public void GetAvailableIngredient_ValidIngredientName_ReturnsIngredient()
        {
            var ingredientName = "Cheese";
            var ingredient = new Ingredient { Name = ingredientName, Price = 0.7M }; // Price in the valid range
            pizzeriaService.AddAvailableIngredient(ingredient);

            var result = pizzeriaService.GetAvailableIngredient(ingredientName);

            Assert.That(result, Is.EqualTo(ingredient));
        }

        [Test]
        public void AddAvailableIngredient_ValidIngredient_AddsIngredient()
        {
            var ingredient = new Ingredient { Name = "Cheese", Price = 0.7M }; // Price in the valid range

            pizzeriaService.AddAvailableIngredient(ingredient);

            var result = pizzeriaService.GetAvailableIngredient(ingredient.Name);

            Assert.That(result, Is.EqualTo(ingredient));
        }

        [Test]
        public void ChangeAvailableIngredient_IngredientHasValidPrice_ChangesIngredient()
        {
            var oldIngredient = new Ingredient { Name = "Cheese", Price = 0.7M }; // Price in the valid range
            var newIngredient = new Ingredient { Name = "Cheese", Price = 0.8M }; // Price in the valid range
            pizzeriaService.AddAvailableIngredient(oldIngredient);

            pizzeriaService.ChangeAvailableIngredient(oldIngredient.Name, newIngredient);

            var result = pizzeriaService.GetAvailableIngredient(oldIngredient.Name);

            Assert.That(result.Price, Is.EqualTo(newIngredient.Price));
        }


        [Test]
        public void ChangeAvailableIngredient_IngredientIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => pizzeriaService.ChangeAvailableIngredient("Cheese", null));
        }

        [Test]
        public void RemoveAvailableIngredient_NameIsEmpty_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => pizzeriaService.RemoveAvailableIngredient(string.Empty));
        }

        [Test]
        public void GetPizza_NameIsEmpty_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => pizzeriaService.GetPizza(string.Empty));
        }

    }
}
