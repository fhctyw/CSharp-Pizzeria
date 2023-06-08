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
        foreach (JObject item in jsonArray.Children<JObject>())
        {
            JToken? ingredientToken;
            if (item.TryGetValue("ingredient", out ingredientToken) && ingredientToken is JObject ingredientObject)
            {
                JToken? nameToken, priceToken;
                if (ingredientObject.TryGetValue("Name", out nameToken) &&
                    ingredientObject.TryGetValue("Price", out priceToken))
                {
                    var ingredient = new Ingredient()
                    {
                        Name = nameToken.ToString(),
                        Price = priceToken.ToObject<decimal>()
                    };

                    JToken? countToken;
                    if (item.TryGetValue("count", out countToken))
                    {
                        dict[ingredient] = countToken.ToObject<int>();
                    }
                    else
                    {
                        Console.WriteLine("Item is missing the 'count' key.");
                    }
                }
                else
                {
                    Console.WriteLine("Ingredient is missing the 'Name' or 'Price' key.");
                }
            }
            else
            {
                Console.WriteLine("Item is missing the 'ingredient' key or it's not an object.");
            }
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
