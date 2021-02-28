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
            switch (_element.ValueKind)
            {
                case JsonValueKind.Array:
                    return GetArrayValue(_element);
                case JsonValueKind.String:
                    return _element.GetString();
                case JsonValueKind.True:
                case JsonValueKind.False:
                    return _element.GetBoolean();
                case JsonValueKind.Number:
                    return _element.GetInt32();
                default:
                    throw new ApplicationException("Invalid json format");
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
