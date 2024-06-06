using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorDesktop.Api;

public interface IPlugin
{
    /// <summary>
    /// 插件初始化
    /// </summary>
    /// <param name="local">运行路径</param>
    /// <param name="type">默认语言</param>
    /// <returns></returns>
    void Init(string local, LanguageType type);
    /// <summary>
    /// 初始化显示实例，你可以在此阶段设置实例的独立配置文件
    /// </summary>
    /// <param name="local">运行路径</param>
    /// <param name="arg">启动参数</param>
    /// <returns>组件实例</returns>
    IInstance MakeInstances(string local, InstanceDataObj arg);
    /// <summary>
    /// 插件停止
    /// </summary>
    void Stop();
    /// <summary>
    /// 打开插件设置
    /// </summary>
    void OpenSetting();
    /// <summary>
    /// 创建一个显示实例，在这里设置显示方式
    /// </summary>
    /// <returns></returns>
    Task<InstanceDataObj> CreateInstances();
}
