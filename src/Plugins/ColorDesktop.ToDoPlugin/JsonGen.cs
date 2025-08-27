using System.Text.Json.Serialization;
using ColorDesktop.ToDoPlugin.Objs;

namespace ColorDesktop.ToDoPlugin;

[JsonSerializable(typeof(OAuthObj))]
[JsonSerializable(typeof(ToDoListObj))]
[JsonSerializable(typeof(ToDoTaskListObj))]
[JsonSerializable(typeof(ToDoInstanceObj))]
[JsonSerializable(typeof(OAuthGetCodeObj))]
public partial class JsonGen : JsonSerializerContext
{
}
