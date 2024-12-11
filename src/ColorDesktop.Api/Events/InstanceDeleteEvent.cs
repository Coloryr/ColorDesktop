using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorDesktop.Api.Events;

public class InstanceDeleteEvent(string plugin, string uuid) : InstanceEvent(plugin, uuid)
{

}
