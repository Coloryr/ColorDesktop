using ColorDesktop.Api.Objs;

namespace ColorDesktop.Web;

public record WebPluginDataObj : PluginDataObj
{
    public string PluginType { get; set; }
}
