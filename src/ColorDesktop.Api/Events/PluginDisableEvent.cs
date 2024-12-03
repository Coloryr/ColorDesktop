using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorDesktop.Api.Events;

/// <summary>
/// 组件禁用
/// </summary>
/// <param name="plugin"></param>
public class PluginDisableEvent(string plugin) : PluginEvent(plugin)
{

}
