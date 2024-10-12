using Avalonia.Controls;
using ColorDesktop.Api;

namespace ColorDesktop.PGLauncherPlugin.ColorMC;

public partial class PGColorMCControl : UserControl, IInstance
{
    public PGColorMCControl()
    {
        InitializeComponent();

        DataContext = new PGColorMCModel();
    }

    public Control CreateView()
    {
        return this;
    }

    public void RenderTick()
    {

    }

    public void Start(IInstanceWindow window)
    {

    }

    public void Stop(IInstanceWindow window)
    {

    }

    public void Update(InstanceDataObj obj)
    {
        var config = PGColorMCPlugin.GetConfig(obj);

        if (DataContext is PGColorMCModel model)
        {
            model.Update(config);
        }
    }
}