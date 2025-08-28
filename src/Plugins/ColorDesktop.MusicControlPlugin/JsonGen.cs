using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ColorDesktop.MusicControlPlugin;

[JsonSerializable(typeof(MusicInstanceObj))]
public partial class JsonGen : JsonSerializerContext
{
}
