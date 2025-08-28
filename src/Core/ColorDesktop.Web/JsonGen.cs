using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ColorDesktop.Api.Objs;

namespace ColorDesktop.Web;

[JsonSerializable(typeof(WebInstanceObj))]
[JsonSerializable(typeof(InstanceDataObj))]
[JsonSerializable(typeof(WebPluginDataObj))]
public partial class JsonGen : JsonSerializerContext
{
}
