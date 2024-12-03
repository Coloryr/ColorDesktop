using Newtonsoft.Json;

namespace ColorDesktop.Api;

public record PluginDependentObj
{
    public string Type { get; set; }
    public string ID { get; set; }
}

public record PluginDataObj
{
    /// <summary>
    /// 唯一ID
    /// </summary>
    public string ID { get; set; }
    /// <summary>
    /// 名字
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 作者
    /// </summary>
    public string Auther { get; set; }
    /// <summary>
    /// 描述
    /// </summary>
    public string Describe { get; set; }
    /// <summary>
    /// 需要加载的Dll库
    /// </summary>
    public List<string> Dlls { get; set; }
    /// <summary>
    /// 依赖其他组件
    /// </summary>
    public List<PluginDependentObj> Dependents { get; set; }
    /// <summary>
    /// 支持的操作系统
    /// </summary>
    public List<string> Os { get; set; }
    /// <summary>
    /// 是否需要权限才能被其他插件控制
    /// </summary>
    public bool Permission { get; set; }
    /// <summary>
    /// 是否支持重载
    /// </summary>
    public bool Reload { get; set; }
    /// <summary>
    /// 版本
    /// </summary>
    public string Version { get; set; }
    /// <summary>
    /// API版本号
    /// </summary>
    public string ApiVersion { get; set; }

    [JsonIgnore]
    public string Local;
}
