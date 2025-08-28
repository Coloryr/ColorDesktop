using System.Text.Json.Serialization;

namespace ColorDesktop.ClockPlugin;

[JsonSerializable(typeof(ClockInstanceObj))]
[JsonSerializable(typeof(ClockConfigObj))]
public partial class JsonGen : JsonSerializerContext
{
}
