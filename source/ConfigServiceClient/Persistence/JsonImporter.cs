using System;
using System.Linq;
using System.Text.Json;
using ConfigServiceClient.Core.Models;

namespace ConfigServiceClient.Persistence
{
    public class JsonImporter : IJsonImporter<IOptionGroup>
    {
        public IOptionGroup ImportFromJson(string json)
        {
            var doc = JsonDocument.Parse(json);
            var rootGroup = new OptionGroup("");
            FillGroup(rootGroup, doc.RootElement);
            return rootGroup;
        }

        private static void FillGroup(IOptionGroupBuilder parent, JsonElement element)
        {
            foreach (var p in element.EnumerateObject())
            {
                if (p.Value.ValueKind == JsonValueKind.Object)
                {
                    var created = parent.AddNested(p.Name);
                    FillGroup(created, p.Value);
                }
                else
                {
                    parent.AddOption(p.Name, GetValue(p));
                }
            }
        }

        private static object GetValue(JsonProperty prop)
        {
            switch (prop.Value.ValueKind)
            {
                case JsonValueKind.Array:
                    return GetArrayValue(prop.Value);
                case JsonValueKind.String:
                    return prop.Value.GetString();
                case JsonValueKind.False:
                case JsonValueKind.True:
                    return prop.Value.GetBoolean();
                case JsonValueKind.Number:
                    return prop.Value.GetInt32();
                default:
                    throw new ApplicationException("Invalid Json format");
            }
        }

        private static object GetArrayValue(JsonElement el)
        {
            var arr = el.EnumerateArray();
            if (!arr.Any())
            {
                return null;
            }

            switch (arr.First().ValueKind)
            {
                case JsonValueKind.String:
                    return arr.Select(x => x.GetString()).ToArray();
                case JsonValueKind.Number:
                    return arr.Select(x => x.GetInt32()).ToArray();
                default:
                    throw new ApplicationException("Invalid Json format");
            }
        }
    }
}