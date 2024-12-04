using ColorDesktop.Api.Objs;

namespace ColorDesktop.Api;

public interface IInstanceManager
{
    IReadOnlyList<string> GetInstances();
    InstanceDataObj? GetInstanceData(string uuid);
    InstanceState? GetState(string uuid);
    public ManagerState Enable(string uuid);
    public ManagerState Disable(string uuid);
    public ManagerState Delete(string uuid);
    public ManagerState Create(InstanceDataObj data);
    public ManagerState SetInstanceData(InstanceDataObj data);
    public IInstanceHandel? GetHandel(string uuid);
}
