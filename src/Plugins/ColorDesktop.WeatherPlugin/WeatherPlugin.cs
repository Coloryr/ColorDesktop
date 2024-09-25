using System.Reflection;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using ColorDesktop.Api;
using ColorDesktop.CoreLib;
using ColorDesktop.WeatherPlugin.Objs;
using Newtonsoft.Json;

namespace ColorDesktop.WeatherPlugin;

public class WeatherPlugin : IPlugin
{
    public const string ConfigName = "weather.json";

    private static string s_local;

    public static List<CityObj> Citys { get; private set; }

    public static WeatherInstanceObj GetConfig(InstanceDataObj obj)
    {
        return InstanceUtils.GetConfig(obj, new WeatherInstanceObj()
        {
            City = "WX4FBXXFKE4F",
            BackColor = "#0294FF",
            TextColor = "#FFFFFF"
        }, ConfigName);
    }

    public static void SaveConfig(InstanceDataObj obj, WeatherInstanceObj config)
    {
        InstanceUtils.SaveConfig(obj, config, ConfigName);
    }

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

    public bool IsCoreLib => false;

    public bool HavePluginSetting => true;

    public bool HaveInstanceSetting => true;

    public InstanceDataObj CreateInstanceDefault()
    {
        return new InstanceDataObj()
        {
            Nick = "天气查询",
            Plugin = "coloryr.weather",
            Pos = PosEnum.TopLeft,
            Margin = new(5)
        };
    }

    public void Disable()
    {

    }

    public void Enable()
    {

    }

    public Bitmap? GetIcon()
    {
        var assm = Assembly.GetExecutingAssembly();
        using var item = assm.GetManifestResourceStream("ColorDesktop.WeatherPlugin.icon.png")!;
        return new Bitmap(item);
    }

    public void Init(string local, string local1, LanguageType type)
    {
        s_local = local;

        var assm = Assembly.GetExecutingAssembly();
        using var item = assm.GetManifestResourceStream("ColorDesktop.WeatherPlugin.Resource.City.json")!;
        using var reader = new JsonTextReader(new StreamReader(item));
        var jsonSerializer = JsonSerializer.CreateDefault();
        Citys = jsonSerializer.Deserialize<List<CityObj>>(reader)!;
    }

    public IInstance MakeInstances(InstanceDataObj obj)
    {
        return new WeatherControl();
    }

    public Control OpenSetting(InstanceDataObj instance)
    {
        return new WeatherSettingControl(instance);
    }

    public Control OpenSetting()
    {
        return new();
    }

    public void Stop()
    {

    }
}
