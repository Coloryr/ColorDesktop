using System.Collections.Generic;
using ColorDesktop.Api.Objs;

namespace ColorDesktop.Launcher.Objs;

public record ConfigObj
{
    public bool AutoStart { get; set; }
    public bool AutoMin { get; set; }
    public WindowTransparencyType Tran { get; set; }
    public List<string> EnablePlugin { get; set; }
    public List<string> EnableInstance { get; set; }
    public List<PluginSourceObj> PluginSource { get; set; }
}
