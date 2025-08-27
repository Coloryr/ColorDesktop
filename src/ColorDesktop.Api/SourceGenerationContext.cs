using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using ColorDesktop.Api.Objs;

namespace ColorDesktop.Api;

[JsonSerializable(typeof(PluginDataObj))]
[JsonSerializable(typeof(InstanceDataObj))]
[JsonSerializable(typeof(GroupObj))]
public partial class SourceGenerationContext : JsonSerializerContext
{

}

public static class JsonType
{
    public static JsonTypeInfo<InstanceDataObj> InstanceDataObj => SourceGenerationContext.Default.InstanceDataObj;
    public static JsonTypeInfo<GroupObj> GroupObj => SourceGenerationContext.Default.GroupObj;
    public static JsonTypeInfo<PluginDataObj> PluginDataObj => SourceGenerationContext.Default.PluginDataObj;
}