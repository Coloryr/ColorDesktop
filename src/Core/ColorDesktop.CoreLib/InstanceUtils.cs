using System.Text.Json.Serialization.Metadata;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;

namespace ColorDesktop.CoreLib;

public static class InstanceUtils
{
    private static readonly Dictionary<string, object> s_configs = [];

    public static T GetConfig<T>(this InstanceDataObj obj, T config, string name, JsonTypeInfo<T> info) where T : new()
    {
        if (s_configs.TryGetValue(obj.UUID, out var data))
        {
            return (T)data!;
        }

        var obj1 = ConfigUtils.Config<T>(config, Path.GetFullPath(CoreLib.InstanceLocal + "/" + obj.UUID + "/" + name), info);

        obj1 ??= config;

        s_configs.Add(obj.UUID, obj1!);

        return obj1;
    }

    public static void SaveConfig<T>(this InstanceDataObj obj, T config, string name, JsonTypeInfo<T> info)
    {
        if (!s_configs.TryAdd(obj.UUID, config!))
        {
            s_configs[obj.UUID] = config!;
        }

        ConfigSave.AddItem(ConfigSaveObj.Build("corelib" + name + obj.UUID,
            Path.GetFullPath(CoreLib.InstanceLocal + "/" + obj.UUID + "/" + name), config, info));
    }
}
