using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorDesktop.WeatherPlugin.Objs;
using Newtonsoft.Json;

namespace ColorDesktop.WeatherPlugin;

public static class AmapApi
{
    public static HttpClient Client = new();

    public static List<City1Obj> Citys { get; set; }

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
        if (city.Citycode != 0)
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
        if (city.Citycode != 0)
        {
            list.Insert(0, city.Name);
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

        return null;
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
        if (city.Citycode != 0)
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

    public static City1Obj? GetCityIndex(int index, int index1, int index2)
    {
        var city1 = GetCityIndex(index);
        var city = GetCityIndex(index, index1);
        if (city == null || city1 == null)
        {
            return null;
        }
        if (city.Citycode != 0)
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

    public static City1Obj? GetCity(City1Obj obj, int adcode)
    {
        if (obj.Adcode == adcode)
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
                if (city1.Citycode != 0)
                {
                    index.Item1++;
                }
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
                if (city1.Citycode != 0)
                {
                    index++;
                }
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

    public static async Task<WeatherInfoObj?> GetData(int code, bool isall)
    {
        try
        {
            var data = await Client.GetStringAsync($"https://restapi.amap.com/v3/weather/weatherInfo?key=ca07fbb30c9d16e146c5652f7dd1faa7&city={code}&extensions={(isall ? "all" : "base")}");

            return JsonConvert.DeserializeObject<WeatherInfoObj>(data);
        }
        catch (Exception e)
        {
            return null;
        }
    }
}
