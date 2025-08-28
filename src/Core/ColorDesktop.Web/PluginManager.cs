using System.Text.Json;
using ColorDesktop.Api;

namespace ColorDesktop.Web;

internal static class PluginManager
{
    public readonly static Dictionary<string, PluginType> PluginTypes = [];
    public static readonly Dictionary<string, WebPluginDataObj> Plugins = [];
    public static readonly Dictionary<string, PluginState> PluginStates = [];

    public const string Dir1 = "plugins";
    public const string ConfigName = "plugin.json";

    public static string RunDir;

    public static void Init(string dir)
    {
        RunDir = Path.GetFullPath(dir + "/" + Dir1);

        Directory.CreateDirectory(RunDir);

        Reload();
    }

    /// <summary>
    /// 设置插件状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="state"></param>
    private static void SetPluginState(string id, PluginState state)
    {
        if (!PluginStates.TryAdd(id, state))
        {
            PluginStates[id] = state;
        }
    }

    public static bool CheckOs(List<string> config)
    {
        if (config == null)
        {
            return true;
        }

        string system;
        if (SystemInfo.Os == OsType.Linux)
        {
            system = "linux_";
        }
        else if (SystemInfo.Os == OsType.MacOS)
        {
            system = "macos_";
        }
        else
        {
            system = "windows_";
        }

        if (SystemInfo.IsArm)
        {
            system += "arm64";
        }
        else
        {
            system += "x86_64";
        }

        return config.Contains(system);
    }

    public static string[] GetPlugins()
    {
        return [.. Plugins.Keys];
    }

    public static void Reload()
    {
        PluginTypes.Clear();
        Plugins.Clear();
        PluginStates.Clear();

        HttpWeb.Clear();

        var list = new HashSet<string>();

        foreach (var item in Directory.GetDirectories(RunDir))
        {
            try
            {
                var list2 = PathHelper.GetAllFile(item);
                var config = list2.FirstOrDefault(item =>
                    item.Name.Equals(ConfigName, StringComparison.CurrentCultureIgnoreCase));
                if (config == null)
                {
                    continue;
                }
                var obj = JsonSerializer.Deserialize<WebPluginDataObj>(File.ReadAllText(config.FullName), JsonGen.Default.WebPluginDataObj);
                if (obj == null)
                {
                    continue;
                }

                if (Plugins.ContainsKey(obj.ID))
                {
                    Logs.Error(string.Format("组件 {0} 存在重复的ID", obj.ID));
                    continue;
                }

                Plugins.Add(obj.ID, obj);

                if (obj.ApiVersion != WebDesktop.ApiVersion)
                {
                    Logs.Error(string.Format("组件 {0} 的API版本不一致", obj.ID));
                    list.Add(obj.ID);
                    SetPluginState(obj.ID, PluginState.LoadError);
                    continue;
                }
                if (!CheckOs(obj.Os))
                {
                    Logs.Error(string.Format("组件 {0} 的系统支持列表不支持该系统", obj.ID));
                    list.Add(obj.ID);
                    SetPluginState(obj.ID, PluginState.OsError);
                    continue;
                }

                if (obj.PluginType == "Web")
                {
                    PluginTypes.Add(obj.ID, PluginType.Web);
                }

                HttpWeb.AddPlugin(obj.ID, config.Directory!.FullName);
            }
            catch (Exception e)
            {
                Logs.Error(string.Format("组件 {0} 加载失败", item), e);
            }
        }
    }
}
