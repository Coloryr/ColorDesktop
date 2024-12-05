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
}
