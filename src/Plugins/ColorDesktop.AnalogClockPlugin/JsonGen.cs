using System.Text.Json.Serialization;

namespace ColorDesktop.AnalogClockPlugin;

[JsonSerializable(typeof(AnalogClockInstanceConfigObj))]
public partial class JsonGen1 : JsonSerializerContext
{
}
