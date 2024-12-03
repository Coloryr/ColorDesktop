using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorDesktop.Api.Events;

/// <summary>
/// 实例创建
/// </summary>
/// <param name="plugin"></param>
/// <param name="uuid"></param>
public class InstanceCreateEvent(string plugin, string uuid) : InstanceEvent(plugin, uuid)
{
}
