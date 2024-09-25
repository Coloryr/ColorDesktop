using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorDesktop.WeatherPlugin.Objs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ColorDesktop.WeatherPlugin;

public static class WebClient
{
    public static HttpClient Client = new();

    private static async Task<(bool, string?, T?)> GetData<T>(string url)
    {
        try
        {
            var data = await Client.GetStringAsync(url);
            var obj = JObject.Parse(data);
            if (obj.ContainsKey("status")
                && obj.ContainsKey("status_code"))
            {
                return (false, obj["status_code"]?.ToString(), default);
            }
            else
            {
                return (true, null, obj.ToObject<T>());
            }
        }
        catch (Exception e)
        {
            return (false, e.ToString(), default);
        }
    }

    public static async Task<NowWeatherObj?> GetNow(CityObj city)
    {
        var data = await GetData<NowWeatherObj>($"https://api.seniverse.com/v3/weather/now.json?key=ShSjFZz4BUMKxaCs2&location={city.ID}&language=zh-Hans&unit=c");
        if (data.Item1 == false)
        {
            return null;
        }

        return data.Item3;
    }
}
