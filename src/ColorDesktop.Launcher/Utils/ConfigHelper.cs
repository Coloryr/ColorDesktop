using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorDesktop.Api;
using ColorDesktop.Launcher.Objs;

namespace ColorDesktop.Launcher.Utils;

public static class ConfigHelper
{
    public static ConfigObj Config { get; private set; }

    public static void LoadConfig()
    {
        Config = ConfigUtils.Config(new ConfigObj()
        {
            EnablePlugin = [],
            EnableInstance = []
        }, AppContext.BaseDirectory + "config.json");

        Config.EnablePlugin ??= [];
        Config.EnableInstance ??= [];
    }

    public static void SaveConfig()
    {
        ConfigSave.AddItem(new ConfigSaveObj()
        {
            Name = "config.json",
            Local = AppContext.BaseDirectory + "config.json",
            Obj = Config
        });
    }

    public static void EnablePlugin(string id)
    {
        if (Config.EnablePlugin.Contains(id))
        {
            return;
        }

        Config.EnablePlugin.Add(id);
        SaveConfig();
    }

    public static void DisablePlugin(string id)
    {
        if (!Config.EnablePlugin.Contains(id))
        {
            return;
        }

        Config.EnablePlugin.Remove(id);
        SaveConfig();
    }

    public static void EnableInstance(string id)
    {
        if (Config.EnableInstance.Contains(id))
        {
            return;
        }

        Config.EnableInstance.Add(id);
        SaveConfig();
    }

    public static void DisableInstance(string id)
    {
        if (!Config.EnableInstance.Contains(id))
        {
            return;
        }

        Config.EnableInstance.Remove(id);
        SaveConfig();
    }
}
