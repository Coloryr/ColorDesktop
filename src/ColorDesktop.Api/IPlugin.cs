using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;

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
    /// 创建一个显示实例默认参数，默认显示方式，启动参数
    /// </summary>
    /// <returns></returns>
    InstanceDataObj CreateInstanceDefault();
    /// <summary>
    /// 显示实例位置设置完成后，进行组件的某些内容设置
    /// 返回空则表示不需要额外设置
    /// </summary>
    /// <returns></returns>
    Window? CreateInstanceSetting(InstanceDataObj data);
}
