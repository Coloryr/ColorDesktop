using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using ColorDesktop.Api;

namespace ColorDesktop.Launcher.Objs;

public record InstanceWindowObj
{
    public Window Window;
    public InstanceDataObj InstanceData;

    public InstanceWindowObj(Window window, InstanceDataObj instanceData)
    {
        Window = window;
        InstanceData = instanceData;
    }
}
