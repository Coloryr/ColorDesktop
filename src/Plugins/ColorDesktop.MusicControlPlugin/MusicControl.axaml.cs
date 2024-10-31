using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ColorDesktop.Api;

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
        
    }
}