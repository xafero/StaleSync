using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace StaleSync.Proto
{
    public static class Protocol
    {
        private static JsonSerializerSettings GetConfig()
            => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                Converters = { new StringEnumConverter() },
                NullValueHandling = NullValueHandling.Ignore
            };

        private static readonly JsonSerializerSettings Config = GetConfig();

        public static Message FromJson(string json)
        {
            var value = JsonConvert.DeserializeObject<Message>(json, Config);
            return value;
        }

        public static string ToJson(Message value)
        {
            var json = JsonConvert.SerializeObject(value, Config);
            return json;
        }
    }
}