using Newtonsoft.Json;

namespace ColorDesktop.WeatherPlugin.Objs;

public record WeatherInfoObj
{
    public record ResultObj
    {
        public record LocationObj
        {
            [JsonProperty("country")]
            public string Country { get; set; }
            [JsonProperty("province")]
            public string Province { get; set; }
            [JsonProperty("city")]
            public string City { get; set; }
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("id")]
            public string Id { get; set; }
        }

        public record NowObj
        {
            [JsonProperty("text")]
            public string Text { get; set; }
            [JsonProperty("temp")]
            public int Temp { get; set; }
            [JsonProperty("feels_like")]
            public int FeelsLike { get; set; }
            [JsonProperty("rh")]
            public int Rh { get; set; }
            [JsonProperty("wind_class")]
            public string WindClass { get; set; }
            [JsonProperty("wind_dir")]
            public string WindDir { get; set; }
            [JsonProperty("uptime")]
            public string Uptime { get; set; }
        }

        public record ForecastObj
        {
            [JsonProperty("text_day")]
            public string TextDay { get; set; }
            [JsonProperty("text_night")]
            public string TextNight { get; set; }
            [JsonProperty("high")]
            public int High { get; set; }
            [JsonProperty("low")]
            public int Low { get; set; }
            [JsonProperty("wc_day")]
            public string WcDay { get; set; }
            [JsonProperty("wd_day")]
            public string WdDay { get; set; }
            [JsonProperty("wc_night")]
            public string WcNight { get; set; }
            [JsonProperty("wd_night")]
            public string WdNight { get; set; }
            [JsonProperty("date")]
            public string Date { get; set; }
            [JsonProperty("week")]
            public string Week { get; set; }
        }

        [JsonProperty("location")]
        public LocationObj Location { get; set; }
        [JsonProperty("now")]
        public NowObj Now { get; set; }
        [JsonProperty("forecasts")]
        public List<ForecastObj> Forecasts { get; set; }
    }

    [JsonProperty("status")]
    public int Status { get; set; }
    [JsonProperty("result")]
    public ResultObj Result { get; set; }
    [JsonProperty("message")]
    public string Message { get; set; }
}
