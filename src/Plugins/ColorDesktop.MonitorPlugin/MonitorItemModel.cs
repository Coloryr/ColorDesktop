using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using LibreHardwareMonitor.Hardware;

namespace ColorDesktop.MonitorPlugin;

public partial class MonitorItemModel : ObservableObject
{
    public float Value = float.NaN;
    public float MinValue = float.NaN;
    public float MaxValue = float.NaN;

    public string Format;
    public string FormatMin;
    public string FormatMax;

    [ObservableProperty]
    private Thickness _margin;
    [ObservableProperty]
    private MonitorDisplayType _monitorDisplay;
    [ObservableProperty]
    private ValueType _valueType;

    [ObservableProperty]
    private bool _haveSensor;

    public MonitorItemObj Obj { get; private set; }

    private ISensor _sensor;
    private string _fmt;

    public bool Demo { get; init; }

    public MonitorItemModel(MonitorItemObj item)
    {
        Obj = item;
        var sensors = MonitorPlugin.GetSensors();
        _sensor = sensors.FirstOrDefault(item1 => item1.Identifier.ToString() == item.Sensor)!;
        HaveSensor = _sensor != null;
        if (!Demo)
        {
            Margin = new Thickness(item.Margin.Left, item.Margin.Top, item.Margin.Right, item.Margin.Bottom);
        }
        else
        {
            Margin = new Thickness(0);
        }
        MonitorDisplay = item.Display;
        ValueType = item.ValueType;

        _fmt = item.Format;

        Update();
    }

    public void Update()
    {
        if (!HaveSensor)
        {
            return;
        }
        try
        {
            float temp;
            if (_sensor.Value is { } value)
            {
                temp = value;
            }
            else
            {
                temp = float.NaN;
            }
            if (Value != temp)
            {
                Value = temp;
                Format = string.Format(_fmt, temp);
            }

            if (_sensor.Min is { } value1)
            {
                temp = value1;
            }
            else
            {
                temp = float.NaN;
            }
            if (MinValue != temp)
            {
                MinValue = temp;
                FormatMin = string.Format(_fmt, temp);
            }

            if (_sensor.Max is { } value2)
            {
                temp = value2;
            }
            else
            {
                temp = float.NaN;
            }
            if (MaxValue != temp)
            {
                MaxValue = temp;
                FormatMax = string.Format(_fmt, temp);
            }
        }
        catch
        {
            HaveSensor = false;
        }
        OnPropertyChanged(nameof(Update));
    }

    public void Reload()
    {
        MonitorDisplay = Obj.Display;
        ValueType = Obj.ValueType;

        OnPropertyChanged(nameof(Reload));
    }

    public void FormatReset()
    {
        _fmt = Obj.Format;

        Value = MinValue = MaxValue = float.NaN;
    }

    public void SensorReset()
    {
        var sensors = MonitorPlugin.GetSensors();
        _sensor = sensors.FirstOrDefault(item1 => item1.Identifier.ToString() == Obj.Sensor)!;
        HaveSensor = _sensor != null;
    }
}
