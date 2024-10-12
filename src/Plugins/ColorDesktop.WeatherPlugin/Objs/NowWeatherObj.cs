using Newtonsoft.Json;

namespace ColorDesktop.WeatherPlugin.Objs;

public record NowWeatherObj
{
    public record ResultObj
    {
        public record LocationObj
        {
            [JsonProperty("id")]
            public string Id { get; set; }
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("country")]
            public string Country { get; set; }
            [JsonProperty("path")]
            public string Path { get; set; }
            [JsonProperty("timezone")]
            public string Timezone { get; set; }
            [JsonProperty("timezone_offset")]
            public string Timezone_offset { get; set; }
        }
        public record NowObj
        {
            [JsonProperty("text")]
            public string Text { get; set; }
            [JsonProperty("code")]
            public string Code { get; set; }
            [JsonProperty("temperature")]
            public string Temperature { get; set; }
        }
        [JsonProperty("location")]
        public LocationObj Location { get; set; }
        [JsonProperty("now")]
        public NowObj Now { get; set; }
        [JsonProperty("last_update")]
        public DateTime LastUpdate { get; set; }
    }
    [JsonProperty("results")]
    public List<ResultObj> Results { get; set; }
}
