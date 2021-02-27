using System.Text.Json.Serialization;

namespace ConfigServiceClient.Core.Models
{
    public sealed class Option
    {
        public string Name { get; set; }
        [JsonPropertyName("value")]
        public object ValueKind { get; set; }

        [JsonIgnore]
        public object Value => new JsonValueParser(ValueKind).Parse();
    }
}
