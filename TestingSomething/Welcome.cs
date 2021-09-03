using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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


    

}
