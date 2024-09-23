using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using ColorDesktop.Api;

namespace ColorDesktop.AnalogClockPlugin;

public partial class PointerClockControl : UserControl, IClock
{
    public PointerClockControl()
    {
        InitializeComponent();

        DataContext = new PointerClockModel();
    }

    public void Tick()
    {
        if (DataContext is PointerClockModel model)
        {
            model.Tick();
        }
    }

    public void Update(AnalogClockConfigObj config)
    {
        if (DataContext is PointerClockModel model)
        {
            model.Update(config);
        }
    }
}