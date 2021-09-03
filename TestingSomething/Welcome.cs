using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;

namespace TestingSomething
{

    public partial class Welcome
    {
        [JsonProperty("Grid")]
        public Grid Grid { get; set; }
    }

    public partial class Grid
    {
        [JsonProperty("DataRow")]
        [JsonConverter(typeof(SafeCollectionConverter))]
        public DataRow[] DataRow { get; set; }
    }

    public partial class DataRow
    {
        [JsonProperty("DataItem")]
        public DataItem[] DataItem { get; set; }
    }

    public partial class DataItem
    {
        [JsonProperty("@name")]
        public string Name { get; set; }

        [JsonProperty("#text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }
    }

    public partial class Test
    {
        public static Welcome FromJson(string json) => JsonConvert.DeserializeObject<Welcome>(json, TestingSomething.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Welcome self) => JsonConvert.SerializeObject(self, TestingSomething.Converter.Settings);
    }

    internal static class Converter
    {

        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    public class SafeCollectionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            //This not works for Populate (on existingValue)
            return serializer.Deserialize<JToken>(reader).ToObjectCollectionSafe(objectType, serializer);
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }


    public static class SafeJsonConvertExtensions
    {
        public static object ToObjectCollectionSafe(this JToken jToken, Type objectType)
        {
            return ToObjectCollectionSafe(jToken, objectType, JsonSerializer.CreateDefault());
        }

        public static object ToObjectCollectionSafe(this JToken jToken, Type objectType, JsonSerializer jsonSerializer)
        {
            var expectArray = typeof(System.Collections.IEnumerable).IsAssignableFrom(objectType);

            if (jToken is JArray jArray)
            {
                if (!expectArray)
                {
                    //to object via singel
                    if (jArray.Count == 0)
                        return JValue.CreateNull().ToObject(objectType, jsonSerializer);

                    if (jArray.Count == 1)
                        return jArray.First.ToObject(objectType, jsonSerializer);
                }
            }
            else if (expectArray)
            {
                //to object via JArray
                return new JArray(jToken).ToObject(objectType, jsonSerializer);
            }

            return jToken.ToObject(objectType, jsonSerializer);
        }

        public static T ToObjectCollectionSafe<T>(this JToken jToken)
        {
            return (T)ToObjectCollectionSafe(jToken, typeof(T));
        }

        public static T ToObjectCollectionSafe<T>(this JToken jToken, JsonSerializer jsonSerializer)
        {
            return (T)ToObjectCollectionSafe(jToken, typeof(T), jsonSerializer);
        }
    }




}
