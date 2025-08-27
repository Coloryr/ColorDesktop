using System.Text.Json.Serialization;
using ColorDesktop.Launcher.Objs;

namespace ColorDesktop.Launcher.Utils;

[JsonSerializable(typeof(PluginDownloadObj))]
[JsonSerializable(typeof(ConfigObj))]
public partial class JsonGen : JsonSerializerContext
{
}
