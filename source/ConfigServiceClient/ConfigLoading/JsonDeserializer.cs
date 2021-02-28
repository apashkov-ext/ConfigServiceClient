using System.Text.Json;

namespace ConfigServiceClient.ConfigLoading
{
    internal static class JsonDeserializer
    {
        public static T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, SerializerOptions.JsonSerializerOptions);
        }
    }
}
