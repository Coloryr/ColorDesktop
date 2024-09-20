using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using ColorDesktop.Api;
using ColorDesktop.Launcher.UI.Windows;

namespace ColorDesktop.Launcher.Objs;

public record InstanceWindowObj
{
    public IInstanceWindow Window;
    public IInstance Instance;
    public InstanceDataObj InstanceData;

    public InstanceWindowObj(IInstanceWindow window, IInstance instance, InstanceDataObj instanceData)
    {
        Window = window;
        Instance = instance;
        InstanceData = instanceData;
    }
}
