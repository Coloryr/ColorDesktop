using ColorDesktop.CoreLib;
using ColorDesktop.WeatherPlugin.Objs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ColorDesktop.WeatherPlugin;

public static class ColorMCApi
{
    public static List<City1Obj> Citys { get; set; }

    public static string[] GetCityName()
    {
        return [.. Citys.Select(item => item.Name)];
    }

    public static List<string> GetCityName(int index)
    {
        var city = Citys[index];
        var list = city.Childs.Select(item => item.Name).ToList();
        if (list.Count == 0)
        {
            return list;
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

        return list;
    }

    public static City1Obj? GetCityAdcode(int adcode)
    {
        foreach (var item in Citys)
        {
            var city1 = GetCity(item, adcode);
            if (city1 != null)
            {
                return city1;
            }
        }

        return Citys[0].Childs[0].Childs[0];
    }

    public static City1Obj? GetCityIndex(int index)
    {
        if (index < Citys.Count)
        {
            return Citys[index];
        }

        return null;
    }

    public static City1Obj? GetCityIndex(int index, int index1)
    {
        var city = GetCityIndex(index);
        if (city == null)
        {
            return null;
        }
        if (index1 < city.Childs.Count)
        {
            return city.Childs[index1];
        }

        return null;
    }

    public static City1Obj? GetCityIndex(int index, int index1, int index2)
    {
        var city1 = GetCityIndex(index);
        var city = GetCityIndex(index, index1);
        if (city == null || city1 == null)
        {
            return null;
        }
        if (index2 < city.Childs.Count)
        {
            return city.Childs[index2];
        }

        return null;
    }

    public static City1Obj? GetCity(City1Obj obj, int adcode)
    {
        if (obj.Adcode != 0 && obj.Adcode == adcode)
        {
            return obj;
        }
        else
        {
            foreach (var item in obj.Childs)
            {
                var city1 = GetCity(item, adcode);
                if (city1 != null)
                {
                    return city1;
                }
            }
        }

        return null;
    }

    public static (int, int, int) GetCityIndexAdcode(int adcode)
    {
        for (int a = 0; a < Citys.Count; a++)
        {
            var city1 = Citys[a];
            if (city1.Adcode == adcode)
            {
                return (a, 0, 0);
            }
            var index = GetCityIndex1Adcode(city1.Childs, adcode);
            if (index.Item1 != -1)
            {
                return (a, index.Item1, index.Item2);
            }
        }

        return (0, 0, 0);
    }

    public static (int, int) GetCityIndex1Adcode(List<City1Obj> list, int adcode)
    {
        for (int i = 0; i < list.Count; i++)
        {
            var city1 = list[i];
            if (city1.Adcode == adcode)
            {
                return (i, 0);
            }

            var index = GetCityIndex2Adcode(city1.Childs, adcode);
            if (index != -1)
            {
                return (i, index);
            }
        }

        return (-1, 0);
    }

    public static int GetCityIndex2Adcode(List<City1Obj> list, int adcode)
    {
        for (int i = 0; i < list.Count; i++)
        {
            var city1 = list[i];
            if (city1.Adcode == adcode)
            {
                return i;
            }
        }

        return -1;
    }

    public static async Task<WeatherInfoObj?> GetData(int code)
    {
        try
        {
            var data = await HttpUtils.Client.GetStringAsync($"https://mc1.coloryr.com:8081/weather?id={code}");

            var obj = JObject.Parse(data);
            if (!obj.TryGetValue("res", out var res) || res?.Type != JTokenType.Integer || res.Value<int>() != 100)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<WeatherInfoObj>(obj["data"]!.ToString());
        }
        catch (Exception e)
        {
            return null;
        }
    }
}
