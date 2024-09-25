using Avalonia.Controls;

namespace ColorDesktop.AnalogClockPlugin;

public partial class DigitalClockControl : UserControl, IClock
{
    public DigitalClockControl()
    {
        InitializeComponent();

        DataContext = new DigitalClockModel();
    }

    public void Tick()
    {
        if (DataContext is DigitalClockModel model)
        {
            model.Tick();
        }
    }

    public void Update(AnalogClockInstanceConfigObj obj)
    {
        if (DataContext is DigitalClockModel model)
        {
            model.Update(obj);
        }
    }
}