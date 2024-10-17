using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using LibreHardwareMonitor.Hardware;

namespace ColorDesktop.MonitorPlugin;

public partial class MonitorItemModel : ObservableObject
{
    [ObservableProperty]
    private float _value = float.NaN;
    [ObservableProperty]
    private float _minValue = float.NaN;
    [ObservableProperty]
    private float _maxValue = float.NaN;

    [ObservableProperty]
    private float _percent = float.NaN;
    [ObservableProperty]
    private float _percentMin = float.NaN;
    [ObservableProperty]
    private float _percentMax = float.NaN;

    [ObservableProperty]
    private bool _haveValue;
    [ObservableProperty]
    private bool _haveMin;
    [ObservableProperty]
    private bool _haveMax;

    [ObservableProperty]
    private string _name;
    [ObservableProperty]
    private float _min;
    [ObservableProperty]
    private float _max;

    [ObservableProperty]
    private string _format;
    [ObservableProperty]
    private string _formatMin;
    [ObservableProperty]
    private string _formatMax;

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
        Name = Obj.Name;
        Min = Obj.Min;
        Max = Obj.Max;

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
                Percent = (temp - Min) / Max * 100;
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
                PercentMin = (temp - Min) / Max * 100;
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
                PercentMax = (temp - Min) / Max * 100;
                FormatMax = string.Format(_fmt, temp);
            }
        }
        catch
        {

        }
        OnPropertyChanged(nameof(Update));
    }

    public void Reload()
    {
        MonitorDisplay = Obj.Display;
        ValueType = Obj.ValueType;
        Name = Obj.Name;
        Min = Obj.Min;
        Max = Obj.Max;

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
