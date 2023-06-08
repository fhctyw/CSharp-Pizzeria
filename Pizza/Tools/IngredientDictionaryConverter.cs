using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pizza;

public class IngredientDictionaryConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return (objectType == typeof(Dictionary<Ingredient, int>));
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        Dictionary<Ingredient, int> dict = new Dictionary<Ingredient, int>();
        JArray jsonArray = JArray.Load(reader);
        foreach (var item in jsonArray.Children<JObject>())
        {
            var ingredient = new Ingredient()
            {
                Name = item["ingredient"]["Name"].ToString(),
                Price = item["ingredient"]["Price"].ToObject<decimal>()
            };
            dict[ingredient] = item["count"].ToObject<int>();
        }
        return dict.Count > 0 ? dict : null;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        var dict = value as Dictionary<Ingredient, int>;

        if (dict != null)
        {
            writer.WriteStartArray();
            foreach (var kvp in dict)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("ingredient");
                serializer.Serialize(writer, kvp.Key);
                writer.WritePropertyName("count");
                writer.WriteValue(kvp.Value);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
        }
        else
        {
            writer.WriteNull();
        }
    }
}
