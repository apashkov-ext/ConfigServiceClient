using System.Text.Json;

namespace ConfigServiceClient.Persistence
{
    internal static class SerializerOptions
    {
        public static JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            AllowTrailingCommas = true
        };
    }
}
