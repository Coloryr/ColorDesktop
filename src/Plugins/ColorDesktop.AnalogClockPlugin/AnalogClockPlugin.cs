﻿using System.Reflection;
using Avalonia.Controls;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;
using ColorDesktop.CoreLib;

namespace ColorDesktop.AnalogClockPlugin;

public class AnalogClockPlugin : IPlugin
{
    public const string ConfigName = "analogclock.json";

    public static AnalogClockInstanceConfigObj GetConfig(InstanceDataObj obj)
    {
        return InstanceUtils.GetConfig(obj, new AnalogClockInstanceConfigObj()
        {
            Size = 100,
            Type = ClockType.Analog,
            Color = "Red",
            TextSize = 120,
            TextColor = "#FFFFFF",
            BackColor = "#0000CD",
            BorderColor = "#EEEEEE"
        }, ConfigName);
    }

    public static void SaveConfig(InstanceDataObj obj, AnalogClockInstanceConfigObj config)
    {
        InstanceUtils.SaveConfig(obj, config, ConfigName);
    }

    public bool HavePluginSetting => false;

    public bool HaveInstanceSetting => true;

    public bool CanCreateInstance => true;

    public bool CanEnable => true;

    public InstanceDataObj CreateInstanceDefault()
    {
        return new InstanceDataObj()
        {
            Nick = LangApi.GetLang("AnalogClock.Name"),
            Plugin = "coloryr.analogclock",
            Pos = PosEnum.TopRight,
            Margin = new(5)
        };
    }

    public void Disable()
    {

    }

    public void Enable()
    {

    }

    public Stream? GetIcon()
    {
        var assm = Assembly.GetExecutingAssembly();
        var item = assm.GetManifestResourceStream("ColorDesktop.AnalogClockPlugin.icon.png")!;
        return item;
    }

    public void Init(string local, string local1)
    {

    }

    public IInstance MakeInstances(InstanceDataObj obj)
    {
        return new AnalogClockControl();
    }

    public InstanceSetting OpenSetting(InstanceDataObj instance, bool isNew)
    {
        return new() { Control = new AnalogClockSettingControl(instance) };
    }

    public InstanceSetting OpenSetting()
    {
        return new();
    }

    public void Stop()
    {

    }

    public void LoadLang(LanguageType type)
    {
        var assm = Assembly.GetExecutingAssembly();
        if (assm == null)
        {
            return;
        }
        string name = type switch
        {
            LanguageType.en_us => "ColorDesktop.AnalogClockPlugin.Resource.Lang.en-us.json",
            _ => "ColorDesktop.AnalogClockPlugin.Resource.Lang.zh-cn.json"
        };
        using var item = assm.GetManifestResourceStream(name)!;
        using var reader = new StreamReader(item);
        LangApi.AddLangs(reader.ReadToEnd());
    }

    public bool Permissions(string key, string permission)
    {
        return false;
    }
}
