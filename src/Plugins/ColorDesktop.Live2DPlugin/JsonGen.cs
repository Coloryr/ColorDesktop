using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ColorDesktop.Live2DPlugin;

[JsonSerializable(typeof(Live2DInstanceObj))]
public partial class JsonGen : JsonSerializerContext
{

}
