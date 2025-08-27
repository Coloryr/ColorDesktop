using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ColorDesktop.ClockPlugin;

namespace ColorDesktop.AnalogClockPlugin;

[JsonSerializable(typeof(AnalogClockInstanceConfigObj))]
public partial class JsonGen1 : JsonSerializerContext
{
}
