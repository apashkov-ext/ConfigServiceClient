using System;
using System.Linq;
using System.Text.Json;

namespace ConfigServiceClient.Core
{
    internal class JsonValueParser
    {
        private readonly JsonElement _element;

        public JsonValueParser(object valueKind)
        {
            if (!(valueKind is JsonElement element)) throw new ApplicationException("Invalid json format");
            _element = element;
        }

        public object Parse()
        {
            return _element.ValueKind switch
            {
                JsonValueKind.Array => GetArrayValue(_element),
                JsonValueKind.String => _element.GetString(),
                JsonValueKind.True => _element.GetBoolean(),
                JsonValueKind.False => _element.GetBoolean(),
                JsonValueKind.Number => _element.GetInt32(),
                _ => throw new ApplicationException("Invalid json format")
            };
        }

        private static object GetArrayValue(JsonElement el)
        {
            var arr = el.EnumerateArray();
            if (!arr.Any())
            {
                return null;
            }

            return arr.First().ValueKind switch
            {
                JsonValueKind.String => arr.Select(x => x.GetString()).ToArray(),
                JsonValueKind.Number => arr.Select(x => x.GetInt32()).ToArray(),
                _ => throw new ApplicationException("Invalid Json format")
            };
        }
    }
}
