using System.Text.Json.Serialization;
using ColorDesktop.MinecraftMotdPlugin.Motd;

namespace ColorDesktop.MinecraftMotdPlugin;

[JsonSerializable(typeof(ServerMotdObj))]
[JsonSerializable(typeof(MotdInstanceObj))]
public partial class JsonGen : JsonSerializerContext
{
}
