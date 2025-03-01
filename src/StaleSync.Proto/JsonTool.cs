using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace StaleSync.Proto
{
    public static class JsonTool
    {
        private static JsonSerializerSettings GetConfig() => new()
        {
            Formatting = Formatting.Indented,
            Converters = { new StringEnumConverter() },
            NullValueHandling = NullValueHandling.Ignore
        };

        private static readonly JsonSerializerSettings Config = GetConfig();

        public static T FromJson<T>(string json)
        {
            var value = JsonConvert.DeserializeObject<T>(json, Config);
            return value;
        }

        public static string ToJson(object value)
        {
            var json = JsonConvert.SerializeObject(value, Config);
            return json;
        }
    }
}