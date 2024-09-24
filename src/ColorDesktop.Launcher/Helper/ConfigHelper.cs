using ColorDesktop.Api;
using ColorDesktop.Launcher.Hook;
using ColorDesktop.Launcher.Objs;
using ColorDesktop.Launcher.Utils;

namespace ColorDesktop.Launcher.Helper;

public static class ConfigHelper
{
    public static ConfigObj Config { get; private set; }

    public static void LoadConfig()
    {
        Config = ConfigUtils.Config(new ConfigObj()
        {
            EnablePlugin = [],
            EnableInstance = []
        }, Program.RunDir + "config.json");

        Config.EnablePlugin ??= [];
        Config.EnableInstance ??= [];
    }

    public static void SaveConfig()
    {
        ConfigSave.AddItem(new ConfigSaveObj()
        {
            Name = "config.json",
            Local = Program.RunDir + "config.json",
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

    public static void SetAutoStart(bool start)
    {
        Config.AutoStart = start;
        SaveConfig();
        if (SystemInfo.Os == OsType.Windows)
        {
            Win32.SetLaunch(start);
        }
    }

    public static void SetAuto(bool value)
    {
        Config.AutoMin = value;
        SaveConfig();
    }
}
