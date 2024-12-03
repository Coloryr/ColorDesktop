using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorDesktop.Api;

public class InstanceEvent(string plugin, string uuid)
{
    /// <summary>
    /// 组件ID
    /// </summary>
    public string Plugin => plugin;
    /// <summary>
    /// 实例UUID
    /// </summary>
    public string UUID => uuid;
}
