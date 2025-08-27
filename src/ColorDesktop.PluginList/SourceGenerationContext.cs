using System.Text.Json.Serialization;
using ColorDesktop.Api.Objs;

namespace ColorDesktop.PluginList;

[JsonSerializable(typeof(PluginDownloadObj))]
[JsonSerializable(typeof(PluginDataObj))]
public partial class SourceGenerationContext : JsonSerializerContext
{

}
