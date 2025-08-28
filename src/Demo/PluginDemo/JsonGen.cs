using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PluginDemo;

[JsonSerializable(typeof(DemoObj))]
public partial class JsonGen : JsonSerializerContext
{

}
