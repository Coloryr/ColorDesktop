using System.Text.Json.Serialization;

namespace ColorDesktop.WeatherPlugin.Objs;

public record WeatherInfoObj
{
    public record ResultObj
    {
        public record LocationObj
        {
            [JsonPropertyName("country")]
            public string Country { get; set; }
            [JsonPropertyName("province")]
            public string Province { get; set; }
            [JsonPropertyName("city")]
            public string City { get; set; }
            [JsonPropertyName("name")]
            public string Name { get; set; }
            [JsonPropertyName("id")]
            public string Id { get; set; }
        }

        public record NowObj
        {
            [JsonPropertyName("text")]
            public string Text { get; set; }
            [JsonPropertyName("temp")]
            public int Temp { get; set; }
            [JsonPropertyName("feels_like")]
            public int FeelsLike { get; set; }
            [JsonPropertyName("rh")]
            public int Rh { get; set; }
            [JsonPropertyName("wind_class")]
            public string WindClass { get; set; }
            [JsonPropertyName("wind_dir")]
            public string WindDir { get; set; }
            [JsonPropertyName("uptime")]
            public string Uptime { get; set; }
        }

        public record ForecastObj
        {
            [JsonPropertyName("text_day")]
            public string TextDay { get; set; }
            [JsonPropertyName("text_night")]
            public string TextNight { get; set; }
            [JsonPropertyName("high")]
            public int High { get; set; }
            [JsonPropertyName("low")]
            public int Low { get; set; }
            [JsonPropertyName("wc_day")]
            public string WcDay { get; set; }
            [JsonPropertyName("wd_day")]
            public string WdDay { get; set; }
            [JsonPropertyName("wc_night")]
            public string WcNight { get; set; }
            [JsonPropertyName("wd_night")]
            public string WdNight { get; set; }
            [JsonPropertyName("date")]
            public string Date { get; set; }
            [JsonPropertyName("week")]
            public string Week { get; set; }
        }

        [JsonPropertyName("location")]
        public LocationObj Location { get; set; }
        [JsonPropertyName("now")]
        public NowObj Now { get; set; }
        [JsonPropertyName("forecasts")]
        public List<ForecastObj> Forecasts { get; set; }
    }

    [JsonPropertyName("status")]
    public int Status { get; set; }
    [JsonPropertyName("result")]
    public ResultObj Result { get; set; }
    [JsonPropertyName("message")]
    public string Message { get; set; }
}
