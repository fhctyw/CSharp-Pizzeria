using Newtonsoft.Json;
using Pizza;

public class JsonFileSerializer : ISerializer
{
    public T Deserialize<T>(string path)
    {
        string json = File.ReadAllText(path);
        JsonSerializerSettings settings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
        settings.Converters.Add(new IngredientDictionaryConverter());
        T? obj = JsonConvert.DeserializeObject<T>(json, settings);
        if (obj == null)
        {
            throw new CannotDeserializeJsonFileException($"Cannot deserialize json {json}");
        }
        return obj;
    }

    public void Serialize<T>(string path, T obj)
    {
        JsonSerializerSettings settings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
        settings.Converters.Add(new IngredientDictionaryConverter());
        string json = JsonConvert.SerializeObject(obj, settings);
        File.WriteAllText(path, json);
    }
}
