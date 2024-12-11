using ColorDesktop.Api;
using ColorDesktop.Api.Objs;

namespace ColorDesktop.Web;

public static class InstanceManager
{

    private static readonly Dictionary<string, object> s_configs = [];


    public static readonly Dictionary<string, WebInstanceObj> Instances = [];
    public static readonly Dictionary<string, InstanceDataObj> InstanceDatas = [];
    public static readonly Dictionary<string, CefBrowserInstance> InstanceCefs = [];

    public static void Init(InstanceDataObj obj, CefBrowserInstance cef)
    {
        var config = GetConfig(obj, new WebInstanceObj(), "webplugin.json");
        if (string.IsNullOrWhiteSpace(config.Plugin))
        {
            throw new Exception("Plugin is null");
        }
        Instances[obj.UUID] = config;
        InstanceCefs[obj.UUID] = cef;
        InstanceDatas[obj.UUID] = obj;
    }

    public static void Remove(string uuid)
    {
        Instances.Remove(uuid);
        InstanceCefs.Remove(uuid);
        InstanceDatas.Remove(uuid);
    }
    public static T GetConfig<T>(InstanceDataObj obj, T config, string name) where T : new()
    {
        if (s_configs.TryGetValue(obj.UUID, out var data))
        {
            return (T)data;
        }

        var obj1 = ConfigUtils.Config<T>(config, WebDesktop.InstanceLocal + "/" + obj.UUID + "/" + name);

        obj1 ??= config;

        s_configs.Add(obj.UUID, obj1!);

        return obj1;
    }

    public static void SaveConfig(InstanceDataObj obj, object config, string name)
    {
        if (!s_configs.TryAdd(obj.UUID, config))
        {
            s_configs[obj.UUID] = config;
        }

        ConfigSave.AddItem(new()
        {
            Name = "webplugin" + name + obj.UUID,
            Local = WebDesktop.InstanceLocal + "/" + obj.UUID + "/" + name,
            Obj = config
        });
    }
}
