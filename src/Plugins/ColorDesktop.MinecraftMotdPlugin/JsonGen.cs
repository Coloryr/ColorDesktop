using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ColorDesktop.MinecraftMotdPlugin.Motd;

namespace ColorDesktop.MinecraftMotdPlugin;

[JsonSerializable(typeof(ServerMotdObj))]
[JsonSerializable(typeof(MotdInstanceObj))]
public partial class JsonGen : JsonSerializerContext
{
}
