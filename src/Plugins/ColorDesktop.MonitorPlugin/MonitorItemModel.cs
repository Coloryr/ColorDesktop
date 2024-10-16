using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using LibreHardwareMonitor.Hardware;

namespace ColorDesktop.MonitorPlugin;

public partial class MonitorItemModel : ObservableObject
{
    [ObservableProperty]
    private float _value;
    [ObservableProperty]
    private float _minValue;
    [ObservableProperty]
    private float _maxValue;

    [ObservableProperty]
    private float _percent;
    [ObservableProperty]
    private float _percentMin;
    [ObservableProperty]
    private float _percentMax;

    [ObservableProperty]
    private bool _haveValue;
    [ObservableProperty]
    private bool _haveMin;
    [ObservableProperty]
    private bool _haveMax;

    public string Name { get; init; }
    public float Min { get; init; }
    public float Max { get; init; }

    [ObservableProperty]
    private string _format;
    [ObservableProperty]
    private string _formatMin;
    [ObservableProperty]
    private string _formatMax;

    public Thickness Margin { get; init; }
    public MonitorDisplayType MonitorDisplay { get; init; }
    public ValueType ValueType { get; init; }

    public bool HaveSensor { get; init; }

    private readonly ISensor sensor;
    private readonly string _fmt;

    public MonitorItemModel(MonitorItemObj item)
    {
        var sensors = MonitorPlugin.GetSensors();
        sensor = sensors.FirstOrDefault(item1 => item1.Identifier.ToString() == item.Sensor)!;
        HaveSensor = sensor != null;
        Name = item.Name;
        Min = item.Min;
        Max = item.Max;
        Margin = new Thickness(item.Margin.Left, item.Margin.Top, item.Margin.Right, item.Margin.Bottom);
        MonitorDisplay = item.Display;
        ValueType = item.ValueType;

        _fmt = item.Format;

        Update();
    }

    public virtual void Update()
    {
        if (!HaveSensor)
        {
            return;
        }

        if (sensor.Value is { } value)
        {
            Value = value;
            Percent = (value - Min) / Max * 100;
            Format = string.Format(_fmt, value);
        }
        if (sensor.Min is { } value1)
        {
            MinValue = value1;
            PercentMin = (value1 - Min) / Max * 100;
            FormatMin = string.Format(_fmt, value1);
        }
        if (sensor.Max is { } value2)
        {
            MaxValue = value2;
            PercentMax = (value2 - Min) / Max * 100;
            FormatMax = string.Format(_fmt, value2);
        }
    }
}
