using Newtonsoft.Json;
using System.Text;

namespace Payanarvorkss.Payanar.Tabless.Api.Utilitiess
{
    public class Utility
    {
        public static string FirstParentId { get { return "00000000-0000-0000-0000-000000000000"; } }
    }
    public class ConcreteTypeConverter<TConcrete> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            //assume we can convert to anything for now
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            //explicitly specify the concrete type we want to create
            return serializer.Deserialize<TConcrete>(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            //use the default serialization - it works fine
            serializer.Serialize(writer, value);
        }
    }
    public static class Extentionss
    {
        public async static Task<T> GetJsonAsync<T>(HttpClient client, string url)
        {
            using var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            using Stream stream = await response.Content.ReadAsStreamAsync();
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                return JsonConvert.DeserializeObject<T>(reader.ReadToEnd(), new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects,
                    NullValueHandling = NullValueHandling.Ignore
                });
            }
        }
    }
}
