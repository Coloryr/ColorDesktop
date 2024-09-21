using ColorDesktop.Api;

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
