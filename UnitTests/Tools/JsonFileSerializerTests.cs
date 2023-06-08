using Pizza;

namespace UnitTests.Tools
{
    [TestFixture]
    public class JsonFileSerializerTests
    {
        private JsonFileSerializer jsonFileSerializer;

        [SetUp]
        public void SetUp()
        {
            jsonFileSerializer = new JsonFileSerializer();
        }

        [Test]
        public void Deserialize_ThrowsFileNotFoundException_WhenFileDoesNotExist()
        {
            Assert.Throws<FileNotFoundException>(() => jsonFileSerializer.Deserialize<object>("nonexistentfile.json"));
        }

        [Test]
        public void Serialize_WritesJsonToFile()
        {
            string path = Path.GetTempFileName();
            var testObject = new { Name = "Test", Value = 123 };

            jsonFileSerializer.Serialize(path, testObject);

            string json = File.ReadAllText(path);
            Assert.IsTrue(json.Contains("Test"));
            Assert.IsTrue(json.Contains("123"));

            File.Delete(path);
        }

        [Test]
        public void Deserialize_CanReadSerializedObject()
        {
            string path = Path.GetTempFileName();
            var testObject = new { Name = "Test", Value = 123 };

            jsonFileSerializer.Serialize(path, testObject);
            var deserializedObject = jsonFileSerializer.Deserialize<dynamic>(path);

            Assert.AreEqual("Test", deserializedObject.Name.ToString());
            Assert.AreEqual("123", deserializedObject.Value.ToString());

            File.Delete(path);
        }
    }
}
