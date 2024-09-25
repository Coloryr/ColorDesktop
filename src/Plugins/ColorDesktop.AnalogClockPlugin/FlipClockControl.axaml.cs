using Avalonia.Controls;

namespace ColorDesktop.AnalogClockPlugin;

public partial class FlipClockControl : UserControl, IClock
{
    public FlipClockControl()
    {
        InitializeComponent();

        DataContext = new FlipClockModel();
    }

    public void Update(AnalogClockInstanceConfigObj obj)
    {
        if (DataContext is FlipClockModel model)
        {
            model.Update(obj);
        }
    }

    public void Tick()
    {
        if (DataContext is FlipClockModel model)
        {
            model.Tick();
        }
    }
}