using Avalonia.Controls;

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

    public void Update(AnalogClockInstanceConfigObj config)
    {
        if (DataContext is PointerClockModel model)
        {
            model.Update(config);
        }
    }
}