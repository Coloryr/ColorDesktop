using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorDesktop.Api.Events;

/// <summary>
/// 实例更新配置
/// </summary>
public class InstanceUpdateEvent(string plugin, string? group, string uuid) : InstanceEvent(plugin, group, uuid)
{

}
