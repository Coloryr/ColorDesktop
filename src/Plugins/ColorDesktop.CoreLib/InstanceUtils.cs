using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorDesktop.Api;

namespace ColorDesktop.CoreLib;

public static class InstanceUtils
{
    private static readonly Dictionary<string, object> s_configs = [];

    public static T GetConfig<T>(InstanceDataObj obj, T config, string name) where T : new()
    {
        if (s_configs.TryGetValue(obj.UUID, out var data))
        {
            return (T)data;
        }

        var obj1 = ConfigUtils.Config<T>(config, CoreLib.InstanceLocal + "/" + obj.UUID + "/" + name);

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
            Name = "corelib" + name + obj.UUID,
            Local = CoreLib.InstanceLocal + "/" + obj.UUID + "/" + name,
            Obj = config
        });
    }
}
