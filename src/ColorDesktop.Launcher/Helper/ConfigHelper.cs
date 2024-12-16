using System;
using System.IO;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;
using ColorDesktop.Launcher.Hook;
using ColorDesktop.Launcher.Objs;
using ColorDesktop.Launcher.UI.Models.Items;
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
            EnableInstance = [],
            PluginSource =
            [
                new()
                {
                    Url = LauncherUtils.Url,
                    Enable = true
                }
            ]
        }, Program.RunDir + "config.json");

        Config.EnablePlugin ??= [];
        Config.EnableInstance ??= [];
        Config.PluginSource ??=
        [
            new()
            {
                Url = LauncherUtils.Url,
                Enable = true
            }
        ];
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
        else if (SystemInfo.Os == OsType.Linux)
        {
            Linux.SetLaunch(start);
        }
    }

    public static void SetAuto(bool value)
    {
        Config.AutoMin = value;
        SaveConfig();
    }

    public static void SetWindowTran(WindowTransparencyType value)
    {
        Config.Tran = value;
        SaveConfig();
    }

    public static void SetSourceEnable(string url, bool value)
    {
        foreach (var item in Config.PluginSource)
        {
            if (item.Url == url)
            {
                item.Enable = value;
                SaveConfig();
                return;
            }
        }
    }

    public static bool HaveSource(string url)
    {
        foreach (var item in Config.PluginSource)
        {
            if (item.Url == url)
            {
                return true;
            }
        }

        return false;
    }

    public static void SetSourceUrl(string url, string newValue)
    {
        foreach (var item in Config.PluginSource)
        {
            if (item.Url == url)
            {
                item.Url = newValue;
                SaveConfig();
                return;
            }
        }
    }

    public static void RemoveSource(string? url)
    {
        foreach (var item in Config.PluginSource)
        {
            if (item.Url == url)
            {
                Config.PluginSource.Remove(item);
                SaveConfig();
                return;
            }
        }
    }

    public static void AddSource(PluginSourceItemModel model)
    {
        foreach (var item in Config.PluginSource)
        {
            if (item.Url == model.Url)
            {
                return;
            }
        }

        Config.PluginSource.Add(new()
        {
            Enable = model.Enable,
            Url = model.Url!
        });

        SaveConfig();
    }

    public static bool TestEula()
    {
        if (File.Exists(AppContext.BaseDirectory + "eula"))
        {
            return File.ReadAllText(AppContext.BaseDirectory + "eula") != "true";
        }

        return true;
    }

    public static void SetEula()
    {
        File.WriteAllText(AppContext.BaseDirectory + "eula", "true");
    }
}
