using ColorDesktop.Api.Objs;

namespace ColorDesktop.Api;

public interface IInstanceManager
{
    /// <summary>
    /// 获取实例列表
    /// </summary>
    /// <returns></returns>
    IReadOnlyList<string> GetInstances();
    /// <summary>
    /// 获取实例设置
    /// </summary>
    /// <param name="uuid"></param>
    /// <returns></returns>
    InstanceDataObj? GetInstanceData(string uuid);
    /// <summary>
    /// 获取实例状态
    /// </summary>
    /// <param name="uuid"></param>
    /// <returns></returns>
    InstanceState? GetState(string uuid);
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
}
