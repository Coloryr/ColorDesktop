using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ColorDesktop.WeatherPlugin.Objs;

public record WeatherInfoObj
{
    public record LiveObj
    {
        [JsonProperty("province")]
        public string Province { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("adcode")]
        public string Adcode { get; set; }
        [JsonProperty("weather")]
        public string Weather { get; set; }
        [JsonProperty("temperature")]
        public string Temperature { get; set; }
        [JsonProperty("winddirection")]
        public string Winddirection { get; set; }
        [JsonProperty("windpower")]
        public string Windpower { get; set; }
        [JsonProperty("humidity")]
        public string Humidity { get; set; }
        [JsonProperty("reporttime")]
        public DateTime Reporttime { get; set; }
        [JsonProperty("temperature_float")]
        public string TemperatureFloat { get; set; }
        [JsonProperty("humidity_float")]
        public string HumidityFloat { get; set; }
    }
    public record ForecastObj
    {
        public record CastObj
        {
            [JsonProperty("date")]
            public string Date { get; set; }
            [JsonProperty("week")]
            public string Week { get; set; }
            [JsonProperty("dayweather")]
            public string Dayweather { get; set; }
            [JsonProperty("nightweather")]
            public string Nightweather { get; set; }
            [JsonProperty("daytemp")]
            public string Daytemp { get; set; }
            [JsonProperty("nighttemp")]
            public string Nighttemp { get; set; }
            [JsonProperty("daywind")]
            public string Daywind { get; set; }
            [JsonProperty("nightwind")]
            public string Nightwind { get; set; }
            [JsonProperty("daypower")]
            public string Daypower { get; set; }
            [JsonProperty("nightpower")]
            public string Nightpower { get; set; }
            [JsonProperty("daytemp_float")]
            public string DaytempFloat { get; set; }
            [JsonProperty("nighttemp_float")]
            public string NighttempFloat { get; set; }
        }
        [JsonProperty("province")]
        public string Province { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("adcode")]
        public string Adcode { get; set; }
        [JsonProperty("reporttime")]
        public DateTime Reporttime { get; set; }
        [JsonProperty("casts")]
        public List<CastObj> Casts { get; set; }
    }
    [JsonProperty("status")]
    public string Status { get; set; }
    [JsonProperty("count")]
    public string Count { get; set; }
    [JsonProperty("info")]
    public string Info { get; set; }
    [JsonProperty("infocode")]
    public string Infocode { get; set; }
    [JsonProperty("lives")]
    public List<LiveObj> Lives { get; set; }
    [JsonProperty("forecasts")]
    public List<ForecastObj> Forecasts { get; set; }
}
