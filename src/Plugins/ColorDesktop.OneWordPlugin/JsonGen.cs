using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ColorDesktop.OneWordPlugin;

[JsonSerializable(typeof(OneWordInstanceObj))]
public partial class JsonGen : JsonSerializerContext
{
}
