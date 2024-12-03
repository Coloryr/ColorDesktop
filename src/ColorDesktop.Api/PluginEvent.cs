using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorDesktop.Api;

public class PluginEvent(string plugin)
{
    /// <summary>
    /// 组件ID
    /// </summary>
    public string Plugin => plugin;
}
