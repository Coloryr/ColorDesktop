using Avalonia.Controls;
using ColorDesktop.Api;

namespace ColorDesktop.ClockPlugin;

public partial class ClockControl : UserControl, IInstance
{
    public ClockControl()
    {
        InitializeComponent();
    }

    public bool Start()
    {
        var time = DateTime.Now;
        Text1.Text = time.Hour.ToString();
        Text2.Text = time.Minute.ToString();
        return true;
    }

    public bool Stop()
    {
        return true;
    }

    public void RenderTick()
    {
        Start();
    }

    public void OpenSetting()
    { 
        
    }

    public Control CreateView()
    {
        return this;
    }
}
