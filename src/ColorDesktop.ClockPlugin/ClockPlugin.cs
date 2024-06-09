using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using ColorDesktop.Api;

namespace ColorDesktop.ClockPlugin;

public class ClockPlugin : IPlugin
{
    public static ClockSettingWindow? ClockSetting;

    public InstanceDataObj CreateInstanceDefault()
    {
        return new InstanceDataObj()
        {
            Nick = "桌面时钟",
            Plugin = "coloryr.clock",
            Pos = PosEnum.TopRight,
            Margin = new(5)
        };
    }

    public Window? CreateInstanceSetting(InstanceDataObj data)
    {
        return null;
    }

    public void Init(string local, LanguageType type)
    {

    }

    public IInstance MakeInstances(string local, InstanceDataObj arg)
    {
        return new ClockControl();
    }

    public void OpenSetting()
    {
        ClockSetting ??= new();
        ClockSetting.Show();
    }

    public void Stop()
    {
        
    }
}
