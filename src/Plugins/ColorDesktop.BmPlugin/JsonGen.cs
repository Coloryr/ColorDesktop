using System.Text.Json.Serialization;

namespace ColorDesktop.BmPlugin;

[JsonSerializable(typeof(List<BmObj>))]
[JsonSerializable(typeof(BmInstanceObj))]
public partial class JsonGen : JsonSerializerContext
{
}
