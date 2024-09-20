using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;

namespace ColorDesktop.Api;

public interface IInstance
{
    /// <summary>
    /// 实例启动
    /// </summary>
    /// <returns></returns>
    bool Start();
    /// <summary>
    /// 实例停止
    /// </summary>
    /// <returns></returns>
    bool Stop();

    /// <summary>
    /// 渲染
    /// </summary>
    void RenderTick();
    /// <summary>
    /// 打开实例设置
    /// </summary>
    void OpenSetting();
    /// <summary>
    /// 创建显示图层
    /// </summary>
    /// <returns></returns>
    Control CreateView();
    /// <summary>
    /// 配置更新
    /// </summary>
    /// <param name="obj"></param>
    void Update(InstanceDataObj obj);
}
