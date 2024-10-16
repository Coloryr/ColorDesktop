using System.Collections.ObjectModel;
using Avalonia.Threading;
using ColorDesktop.Api;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibreHardwareMonitor.Hardware;

namespace ColorDesktop.MonitorPlugin;

public partial class MonitorInstanceSettingModel : ObservableObject
{
    public partial class ItemNameModel : ObservableObject
    {
        [ObservableProperty]
        private string _name;
    }

    public partial class SensorDataModel : ObservableObject
    {
        public ISensor Sensor;

        public string Name => Sensor.Name;
        public string Type => Sensor.SensorType.ToString();
        public string Value => Sensor.Value?.ToString("0.00") ?? "N/A";
        public string MinValue => Sensor.Min?.ToString("0.00") ?? "N/A";
        public string MaxValue => Sensor.Max?.ToString("0.00") ?? "N/A";

        public void Update()
        {
            OnPropertyChanged(nameof(Value));
            OnPropertyChanged(nameof(MinValue));
            OnPropertyChanged(nameof(MaxValue));
        }
    }

    public string[] PanelTypeName { get; init; } = [LangApi.GetLang("MonitorPanelType.Type1"), LangApi.GetLang("MonitorPanelType.Type2"), LangApi.GetLang("MonitorPanelType.Type3")];

    public string[] DisplayTypeName { get; init; } = [LangApi.GetLang("MonitorPanelType.Type1"), LangApi.GetLang("MonitorPanelType.Type2"), LangApi.GetLang("MonitorPanelType.Type3")];

    public string[] ValueTypeName { get; init; } = [LangApi.GetLang("MonitorPanelType.Type1"), LangApi.GetLang("MonitorPanelType.Type2"), LangApi.GetLang("MonitorPanelType.Type3")];

    public ObservableCollection<ItemNameModel> Items { get; init; } = [];
    public ObservableCollection<SensorDataModel> Sensors { get; init; } = [];

    [ObservableProperty]
    private MonitorItemModel _model;

    [ObservableProperty]
    private int _width;
    [ObservableProperty]
    private int _height;
    [ObservableProperty]
    private bool _autoSize;

    [ObservableProperty]
    private PanelType _panel;

    [ObservableProperty]
    private int _index = -1;

    [ObservableProperty]
    private string _name;
    [ObservableProperty]
    private int _left;
    [ObservableProperty]
    private int _top;
    [ObservableProperty]
    private int _right;
    [ObservableProperty]
    private int _bottom;
    [ObservableProperty]
    private MonitorDisplayType _displayType;
    [ObservableProperty]
    private ValueType _valueType;
    [ObservableProperty]
    private int _valueWidth;
    [ObservableProperty]
    private int _valueHeight;
    [ObservableProperty]
    private SensorDataModel? _selectItem;
    [ObservableProperty]
    private int _fontSize;
    [ObservableProperty]
    private string _format;

    [ObservableProperty]
    private bool _enableItem;

    [ObservableProperty]
    private bool _displaySize;
    [ObservableProperty]
    private bool _displayFontSize;

    private bool _load;
    private bool _run = true;

    private readonly MonitorInstanceObj _config;
    private readonly InstanceDataObj _obj;

    public MonitorInstanceSettingModel(InstanceDataObj obj)
    {
        _obj = obj;
        _config = MonitorPlugin.GetConfig(obj);

        _width = _config.Width;
        _height = _config.Height;
        _autoSize = _config.AutoSize;
        _panel = _config.PanelType;

        foreach (var item in _config.Items)
        {
            Items.Add(new()
            {
                Name = item.Name
            });
        }

        Refrsh();

        if (Items.Count > 0)
        {
            Index = 0;
        }

        DispatcherTimer.Run(() =>
        {
            Update();
            return _run;
        }, TimeSpan.FromSeconds(1));
    }

    partial void OnValueTypeChanged(ValueType value)
    {
        if (_load)
        {
            return;
        }
        var item = _config.Items[Index];
        item.ValueType = value;
        MonitorPlugin.SaveConfig(_obj, _config);
    }

    partial void OnLeftChanged(int value)
    {
        if (_load)
        {
            return;
        }
        var item = _config.Items[Index];
        item.Margin.Left = value;
        MonitorPlugin.SaveConfig(_obj, _config);
    }

    partial void OnTopChanged(int value)
    {
        if (_load)
        {
            return;
        }
        var item = _config.Items[Index];
        item.Margin.Top = value;
        MonitorPlugin.SaveConfig(_obj, _config);
    }

    partial void OnRightChanged(int value)
    {
        if (_load)
        {
            return;
        }
        var item = _config.Items[Index];
        item.Margin.Right = value;
        MonitorPlugin.SaveConfig(_obj, _config);
    }

    partial void OnBottomChanged(int value)
    {
        if (_load)
        {
            return;
        }
        var item = _config.Items[Index];
        item.Margin.Bottom = value;
        MonitorPlugin.SaveConfig(_obj, _config);
    }

    partial void OnNameChanged(string value)
    {
        if (_load)
        {
            return;
        }
        var item = _config.Items[Index];
        item.Name = value;
        Items[Index].Name = value;
        MonitorPlugin.SaveConfig(_obj, _config);
    }

    partial void OnDisplayTypeChanged(MonitorDisplayType value)
    {
        DisplaySetting(value);
        if (_load)
        {
            return;
        }
        var item = _config.Items[Index];
        item.Display = value;
        MonitorPlugin.SaveConfig(_obj, _config);
    }

    partial void OnValueWidthChanged(int value)
    {
        if (_load)
        {
            return;
        }
        var item = _config.Items[Index];
        item.Width = value;
        MonitorPlugin.SaveConfig(_obj, _config);
    }

    partial void OnValueHeightChanged(int value)
    {
        if (_load)
        {
            return;
        }
        var item = _config.Items[Index];
        item.Height = value;
        MonitorPlugin.SaveConfig(_obj, _config);
    }

    partial void OnSelectItemChanged(SensorDataModel? value)
    {
        if (_load || value == null)
        {
            return;
        }
        var item = _config.Items[Index];
        item.Sensor = value.Sensor.Identifier.ToString();
        MonitorPlugin.SaveConfig(_obj, _config);
    }

    partial void OnFormatChanged(string value)
    {
        if (_load)
        {
            return;
        }
        var item = _config.Items[Index];
        item.Format = value;
        MonitorPlugin.SaveConfig(_obj, _config);
    }

    partial void OnIndexChanged(int value)
    {
        if (value == -1)
        {
            EnableItem = false;
            return;
        }
        _load = true;

        var item = _config.Items[value];
        Name = item.Name;
        Left = item.Margin.Left;
        Top = item.Margin.Top;
        Right = item.Margin.Right;
        Bottom = item.Margin.Bottom;
        DisplayType = item.Display;
        ValueType = item.ValueType;
        ValueWidth = item.Width;
        ValueHeight = item.Height;
        Format = item.Format;
        SelectItem = Sensors.FirstOrDefault(item1 => item1.Sensor.Identifier.ToString() == item.Sensor);

        DisplaySetting(DisplayType);

        _load = false;
        EnableItem = true;

        Model = new(item);
    }

    [RelayCommand]
    public void NewItem()
    {
        var item = MonitorPlugin.MakeNewItem();
        Items.Add(new()
        {
            Name = item.Name
        });
        _config.Items.Add(item);
        MonitorPlugin.SaveConfig(_obj, _config);
        Index = Items.Count - 1;
    }

    [RelayCommand]
    public void DeleteItem()
    {
        if (Index == -1)
        {
            return;
        }

        _config.Items.RemoveAt(Index);
        Items.RemoveAt(Index);
        MonitorPlugin.SaveConfig(_obj, _config);
        if (Index == Items.Count)
        {
            Index--;
        }
    }

    [RelayCommand]
    public void Refrsh()
    {
        Sensors.Clear();
        foreach (var sensor in MonitorPlugin.GetSensors())
        {
            Sensors.Add(new()
            {
                Sensor = sensor
            });
        }
    }

    private void DisplaySetting(MonitorDisplayType value)
    {
        switch (value)
        {
            case MonitorDisplayType.Text:
                DisplaySize = true;
                DisplayFontSize = true;
                break;
        }
    }

    private void Update()
    {
        foreach (var item in Sensors)
        {
            item.Update();
        }
    }

    public void Stop()
    {
        _run = false;
    }
}
