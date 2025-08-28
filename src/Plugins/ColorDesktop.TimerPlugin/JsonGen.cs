using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ColorDesktop.TimerPlugin;

[JsonSerializable(typeof(TimerInstanceObj))]
public partial class JsonGen : JsonSerializerContext
{
}
