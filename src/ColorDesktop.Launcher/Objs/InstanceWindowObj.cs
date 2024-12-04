using ColorDesktop.Api;
using ColorDesktop.Api.Objs;

namespace ColorDesktop.Launcher.Objs;

public record InstanceWindowObj
{
    public IInstanceWindow Window;
    public IInstance Instance;
    public InstanceDataObj InstanceData;

    public InstanceWindowObj(IInstanceWindow window, IInstance instance, InstanceDataObj instanceData)
    {
        Window = window;
        Instance = instance;
        InstanceData = instanceData;
    }
}
