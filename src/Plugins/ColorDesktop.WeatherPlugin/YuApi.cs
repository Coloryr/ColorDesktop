using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorDesktop.WeatherPlugin.Objs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ColorDesktop.WeatherPlugin;

public static class YuApi
{
    public static HttpClient Client = new();

    public static List<CityObj> Citys { get; set; }

    public static string[] GetCityName()
    {
        return Citys.Select(item => item.Name).ToArray();
    }

    public static List<string> GetCityName(int index)
    {
        var city = Citys[index];
        var list = city.Childs.Select(item => item.Name).ToList();
        if (list.Count == 0)
        {
            return list;
        }
        if (city.ID != null)
        {
            list.Insert(0, city.Name);
        }

        return list;
    }

    public static List<string> GetCityName(int index, int index1)
    {
        var city = Citys[index].Childs[index1];
        var list = city.Childs.Select(item => item.Name).ToList();
        if (list.Count == 0)
        {
            return list;
        }
        if (city.ID != null)
        {
            list.Insert(0, city.Name);
        }

        return list;
    }

    public static CityObj? GetCity(string city)
    {
        foreach (var item in Citys)
        {
            var city1 = GetCity(item, city);
            if (city1 != null)
            {
                return city1;
            }
        }

        return null;
    }

    public static CityObj? GetCity(int index)
    {
        if (index < Citys.Count)
        {
            return Citys[index];
        }

        return null;
    }

    public static CityObj? GetCity(int index, int index1)
    {
        var city = GetCity(index);
        if (city == null)
        {
            return null;
        }
        if (city.ID != null)
        {
            if (index1 == 0)
            {
                return city;
            }
            else
            {
                index1--;
            }
        }
        if (index1 < city.Childs.Count)
        {
            return city.Childs[index1];
        }

        return null;
    }

    public static CityObj? GetCity(int index, int index1, int index2)
    {
        var city1 = GetCity(index);
        var city = GetCity(index, index1);
        if (city == null || city1 == null)
        {
            return null;
        }
        if (city.ID != null)
        {
            if (index2 == 0)
            {
                return city;
            }
            else
            {
                index2--;
            }
        }
        if (index2 < city.Childs.Count)
        {
            return city.Childs[index2];
        }

        return null;
    }

    public static CityObj? GetCity(CityObj obj, string city)
    {
        if (obj.ID == city)
        {
            return obj;
        }
        else
        {
            foreach (var item in obj.Childs)
            {
                var city1 = GetCity(item, city);
                if (city1 != null)
                {
                    return city1;
                }
            }
        }

        return null;
    }

    public static (int, int, int) GetCityIndex(string city)
    {
        for (int a = 0; a < Citys.Count; a++)
        {
            var city1 = Citys[a];
            if (city1.ID == city)
            {
                return (a, 0, 0);
            }
            var index = GetCityIndex1(city1.Childs, city);
            if (index.Item1 != -1)
            {
                if (city1.ID != null)
                {
                    index.Item1++;
                }
                return (a, index.Item1, index.Item2);
            }
        }

        return (0, 0, 0);
    }

    public static (int, int) GetCityIndex1(List<CityObj> list, string city)
    {
        for (int i = 0; i < list.Count; i++)
        {
            var city1 = list[i];
            if (city1.ID == city)
            {
                return (i, 0);
            }

            var index = GetCityIndex2(city1.Childs, city);
            if (index != -1)
            {
                if (city1.ID != null)
                {
                    index++;
                }
                return (i, index);
            }
        }

        return (-1, 0);
    }

    public static int GetCityIndex2(List<CityObj> list, string city)
    {
        for (int i = 0; i < list.Count; i++)
        {
            var city1 = list[i];
            if (city1.ID == city)
            {
                return i;
            }
        }

        return -1;
    }

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
