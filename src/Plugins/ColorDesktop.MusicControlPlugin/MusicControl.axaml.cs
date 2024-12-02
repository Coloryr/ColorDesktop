using Avalonia.Controls;
using ColorDesktop.Api;
using ColorDesktop.MusicControlPlugin.Skin1;

namespace ColorDesktop.MusicControlPlugin;

public partial class MusicControl : UserControl, IInstance
{
    public MusicControl()
    {
        InitializeComponent();

        DataContext = new MusicModel();
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
        if (DataContext is MusicModel model)
        {
            model.Init();
        }
    }

    public void Stop(IInstanceWindow window)
    {
        if (DataContext is MusicModel model)
        {
            model.Stop();
        }
    }

    public void Update(InstanceDataObj obj)
    {
        var config = MusicControlPlugin.GetConfig(obj);
        Width = config.Width;

        if (config.Skin == SkinType.Skin1
            && Content is not Skin1Control)
        {
            Content = new Skin1Control();
        }
    }

    public IInstanceHandel? GetHandel()
    {
        return null;
    }
}