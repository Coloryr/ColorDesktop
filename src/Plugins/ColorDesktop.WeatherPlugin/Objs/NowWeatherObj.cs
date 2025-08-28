using System.Text.Json.Serialization;

namespace ColorDesktop.WeatherPlugin.Objs;

public record NowWeatherObj
{
    public record ResultObj
    {
        public record LocationObj
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }
            [JsonPropertyName("name")]
            public string Name { get; set; }
            [JsonPropertyName("country")]
            public string Country { get; set; }
            [JsonPropertyName("path")]
            public string Path { get; set; }
            [JsonPropertyName("timezone")]
            public string Timezone { get; set; }
            [JsonPropertyName("timezone_offset")]
            public string Timezone_offset { get; set; }
        }
        public record NowObj
        {
            [JsonPropertyName("text")]
            public string Text { get; set; }
            [JsonPropertyName("code")]
            public string Code { get; set; }
            [JsonPropertyName("temperature")]
            public string Temperature { get; set; }
        }
        [JsonPropertyName("location")]
        public LocationObj Location { get; set; }
        [JsonPropertyName("now")]
        public NowObj Now { get; set; }
        [JsonPropertyName("last_update")]
        public DateTime LastUpdate { get; set; }
    }
    [JsonPropertyName("results")]
    public List<ResultObj> Results { get; set; }
}
