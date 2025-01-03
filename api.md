# API版本说明
这里的API指的是`ColorDesktop.Api`

# Version5
`InstanceDataObj` 新增
```C#
/// <summary>
/// 备注
/// </summary>
public string Comment { get; set; }
/// <summary>
/// 鼠标穿透
/// </summary>
public bool MouseThrough { get; set; }
```

`InstanceEvent` 新增
```C#
/// <summary>
/// 实例分组
/// </summary>
public string? Group => group;
```

`IInstanceManager` 新增
```C#
/// <summary>
/// 获取实例分组列表
/// </summary>
/// <returns></returns>
IReadOnlyList<string> GetGroups();
/// <summary>
/// 获取当前实例分组
/// </summary>
/// <returns>当前分组UUID，null为默认分组</returns>
string? GetNowGroup();
/// <summary>
/// 切换实例分组
/// </summary>
/// <param name="uuid">分组UUID</param>
void SwitchGroup(string? uuid);
/// <summary>
/// 获取实例分组信息
/// </summary>
/// <param name="uuid">分组UUID</param>
/// <returns>分组信息</returns>
GroupObj? GetGroupObj(string uuid);
/// <summary>
/// 编辑实例分组信息
/// 操作其他组件的实例需要先申请权限
/// </summary>
/// <param name="uuid">实例UUID</param>
/// <param name="type">操作类型</param>
/// <returns>操作结果</returns>
ManagerState EditGroup(string uuid, GroupEditType type);
/// <summary>
/// 创建实例分组
/// </summary>
/// <param name="name">分组名字</param>
/// <returns>分组UUID，空为创建失败</returns>
string? CreateGroup(string name);
/// <summary>
/// 删除实例分组
/// </summary>
/// <param name="uuid">分组UUID</param>
/// <returns></returns>
ManagerState DeleteGroup(string uuid);
```

新增枚举
```C#
/// <summary>
/// 实例分组操作
/// </summary>
public enum GroupEditType
{ 
    /// <summary>
    /// 添加
    /// </summary>
    Add,
    /// <summary>
    /// 删除
    /// </summary>
    Remove,
    /// <summary>
    /// 启用
    /// </summary>
    Enable,
    /// <summary>
    /// 禁用
    /// </summary>
    Disable
}
```

新增事件
```C#
/// <summary>
/// 实例分组创建
/// </summary>
public class GroupAddEvent(string? group);
/// <summary>
/// 分组删除事件
/// </summary>
/// <param name="group"></param>
public class GroupDeleteEvent(string? group);
/// <summary>
/// 分组切换事件
/// </summary>
/// <param name="group"></param>
public class GroupSwitchEvent(string? group);
```

# Version4
`IInstance` 添加新接口
```C#
/// <summary>
/// 窗口显示后
/// </summary>
void WindowLoaded(IInstanceWindow window);
```
修改接口
```C#
/// <summary>
/// 渲染
/// </summary>
public void RenderTick(IInstanceWindow window);
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

# Version3
- 添加事件系统
- 添加公共数据存储
- 添加权限系统
- 添加管理器

# Version2
- 添加语言设置