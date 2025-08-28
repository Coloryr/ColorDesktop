using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ColorDesktop.MonitorPlugin;

[JsonSerializable(typeof(MonitorInstanceObj))]
[JsonSerializable(typeof(MonitorConfigObj))]
public partial class JsonGen : JsonSerializerContext
{
}
