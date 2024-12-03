using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorDesktop.Api.Events;

/// <summary>
/// 组件启用
/// </summary>
/// <param name="plugin"></param>
public class PluginEnableEvent(string plugin) : PluginEvent(plugin)
{

}
