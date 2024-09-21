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
        var time = ClockPlugin.Config.Ntp ? NtpClient.Date : DateTime.Now;
        Text1.Text = string.Format("{0:D2}", time.Hour);
        Text2.Text = string.Format("{0:D2}", time.Minute);
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

    public void Update(InstanceDataObj obj)
    {
        
    }
}
