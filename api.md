# API版本说明
这里的API指的是`ColorDesktop.Api`

# Version 4
`IInstance` 添加新接口
```C#
/// <summary>
/// 窗口显示后
/// </summary>
void WindowLoaded(IInstanceWindow window);
```

`IInstanceWindow` 添加新接口
```C#
/// <summary>
/// 移动
/// </summary>
/// <param name="x"></param>
/// <param name="y"></param>
void Move(int x, int y);
/// <summary>
/// 设置窗口状态
/// </summary>
/// <param name="state"></param>
/// <returns></returns>
void SetState(WindowState state);
/// <summary>
/// 设置窗口透明
/// </summary>
/// <param name="level"></param>
/// <returns></returns>
void SetTran(WindowTransparencyType level);
```

修改 `IPlugin`  
属性修改为
```C#
/// <summary>
/// 是否可以控制开关
/// 为false时不能从组件设置里面手动禁用启用
/// </summary>
public bool CanEnable { get; }
/// <summary>
/// 是否可以创建实例
/// </summary>
public bool CanCreateInstance { get; }
/// <summary>
/// 是否有组件设置
/// </summary>
public bool HavePluginSetting { get; }
/// <summary>
/// 是否有实例设置
/// </summary>
public bool HaveInstanceSetting { get; }
```
打开组件/实例设置入口修改
```C#
/// <summary>
/// 打开实例设置
/// </summary>
/// <param name="instance">实例信息</param>
/// <param name="isNew">是否为新建</param>
InstanceSetting OpenSetting(InstanceDataObj instance, bool isNew);
/// <summary>
/// 打开组件设置
/// </summary>
InstanceSetting OpenSetting();
```

新增 `InstanceSetting` 组件/实例 设置接口
```C#
public class InstanceSetting
{
    /// <summary>
    /// 需要显示的控件
    /// </summary>
    public Control? Control;
    /// <summary>
    /// 完成设置回调
    /// </summary>
    public Action? Close;
}
```

# Version 3
- 添加事件系统
- 添加公共数据存储
- 添加权限系统
- 添加管理器

# Version 2
- 添加语言设置