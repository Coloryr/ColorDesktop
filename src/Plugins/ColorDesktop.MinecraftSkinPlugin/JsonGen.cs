using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ColorDesktop.MinecraftSkinPlugin;

[JsonSerializable(typeof(SkinInstanceObj))]
public partial class JsonGen : JsonSerializerContext
{
}
