# 更新日志

## A4.20250102
- 添加鼠标穿透与实例注释  
- 添加实例分组
- 修改组件与实例界面

## A3.20241213
- 删除ASP强依赖

## A3.20241211
- 新增浏览器组件支持  
[使用教程](./web.md)  

大幅度修改API，将API版本号修改为Version4

## A3.20241205
A3首个测试版

- 本次更新修改了API版本
- 修改了组件与实例的逻辑
- 组件添加了操作系统限制
```json
{
    "Os": [
        "windows_x86_64",
        "windows_arm64",
        "linux_x86_64",
        "linux_arm64",
        "macos_x86_64",
        "macos_arm64"
    ],
}
```
- 组件添加了控制权限系统
```json
{
    "Permission": true,
}
```
```C#
/// <summary>
/// 请求控制组件
/// </summary>
/// <param name="key">组件ID</param>
/// <param name="permission">权限</param>
/// <returns>是否有权限控制</returns>
bool? GetControlTest(string key, string permission);
```
- 组件添加了操作其他组件与实例
```C#
/// <summary>
/// 禁用组件
/// </summary>
/// <param name="key">组件ID</param>
ManagerState Disable(string key);
/// <summary>
/// 启用组件
/// </summary>
/// <param name="key">组件ID</param>
ManagerState Enable(string key);
```
```C#
/// <summary>
/// 启用实例
/// </summary>
/// <param name="uuid">实例UUID</param>
/// <returns></returns>
ManagerState Enable(string uuid);
/// <summary>
/// 禁用实例
/// </summary>
/// <param name="uuid">实例UUID</param>
/// <returns></returns>
ManagerState Disable(string uuid);
/// <summary>
/// 删除实例
/// </summary>
/// <param name="uuid">实例UUID</param>
/// <returns></returns>
ManagerState Delete(string uuid);
/// <summary>
/// 创建实例
/// </summary>
/// <param name="data">实例信息</param>
/// <returns></returns>
ManagerState Create(InstanceDataObj data);
/// <summary>
/// 设置实例信息
/// </summary>
/// <param name="data">实例信息</param>
/// <returns></returns>
ManagerState SetInstanceData(InstanceDataObj data);
/// <summary>
/// 获取实例控制器
/// </summary>
/// <param name="uuid">实例UUID</param>
/// <returns></returns>
IInstanceHandel? GetHandel(string uuid);
```
- 组件添加了事件系统
```C#
/// <summary>
/// 监听事件
/// </summary>
void AddListener(IPlugin plugin, Action<BaseEvent> action);
/// <summary>
/// 取消所有监听
/// </summary>
/// <param name="id"></param>
void RemoveListener(string id);
/// <summary>
/// 触发事件
/// </summary>
/// <param name="pluginEvent"></param>
void CallEvent(BaseEvent pluginEvent);
```
- 组件添加了公共数据存储
```C#
/// <summary>
/// 公共数据获取，只能存基础类型
/// </summary>
/// <param name="key">键</param>
/// <param name="obj">值</param>
/// <returns>是否有数据</returns>
bool GetData(string key, out object? obj);
/// <summary>
/// 公共数据设置，只能存基础类型
/// </summary>
/// <param name="key">键</param>
/// <param name="obj">值</param>
void SetData(string key, object? obj);
/// <summary>
/// 公共数据删除
/// </summary>
/// <param name="key">键</param>
/// <returns>是否成功删除</returns>
bool RemoveData(string key);
```

本次不兼容旧版组件需要更新