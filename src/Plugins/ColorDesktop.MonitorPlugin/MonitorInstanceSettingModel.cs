using System.Collections.ObjectModel;
using Avalonia.Media;
using Avalonia.Threading;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;
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

        public string SensorId => Sensor.Identifier.ToString();
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

    public string[] PanelTypeName { get; init; } =
    [
        LangApi.GetLang("MonitorInstanceSetting.Text29"),
        LangApi.GetLang("MonitorInstanceSetting.Text30"),
        LangApi.GetLang("MonitorInstanceSetting.Text31")
    ];

    public string[] DisplayTypeName { get; init; } =
    [
        LangApi.GetLang("MonitorInstanceSetting.Text32"),
        LangApi.GetLang("MonitorInstanceSetting.Text33"),
        LangApi.GetLang("MonitorInstanceSetting.Text34"),
        LangApi.GetLang("MonitorInstanceSetting.Text40"),
        LangApi.GetLang("MonitorInstanceSetting.Text43"),
        LangApi.GetLang("MonitorInstanceSetting.Text44")
    ];

    public string[] ValueTypeName { get; init; } =
    [
        LangApi.GetLang("MonitorInstanceSetting.Text35"),
        LangApi.GetLang("MonitorInstanceSetting.Text36"),
        LangApi.GetLang("MonitorInstanceSetting.Text37")
    ];

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
    private Color _backColor;
    [ObservableProperty]
    private Color _fontColor;
    [ObservableProperty]
    private int _borderSize;
    [ObservableProperty]
    private Color _barColor;
    [ObservableProperty]
    private Color _barBackColor;
    [ObservableProperty]
    private float _max;
    [ObservableProperty]
    private float _min;

    [ObservableProperty]
    private bool _enableItem;

    [ObservableProperty]
    private bool _displaySize;
    [ObservableProperty]
    private bool _displayFontSize;
    [ObservableProperty]
    private bool _displayFmt;
    [ObservableProperty]
    private bool _displayColor;
    [ObservableProperty]
    private bool _displayBorder;
    [ObservableProperty]
    private bool _displayBarColor;
    [ObservableProperty]
    private bool _displayMaxMin;
    [ObservableProperty]
    private bool _fmtError;

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

    partial void OnAutoSizeChanged(bool value)
    {
        _config.AutoSize = value;
        MonitorPlugin.SaveConfig(_obj, _config);
    }

    partial void OnWidthChanged(int value)
    {
        _config.Width = value;
        MonitorPlugin.SaveConfig(_obj, _config);
    }

    partial void OnHeightChanged(int value)
    {
        _config.Height = value;
        MonitorPlugin.SaveConfig(_obj, _config);
    }

    partial void OnPanelChanged(PanelType value)
    {
        _config.PanelType = value;
        MonitorPlugin.SaveConfig(_obj, _config);
    }

    partial void OnValueTypeChanged(ValueType value)
    {
        if (_load)
        {
            return;
        }
        var item = _config.Items[Index];
        item.ValueType = value;
        Model?.Reload();
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
        Model?.Reload();
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
        Model = null;
        Model = new(item);
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
        Model?.Reload();
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
        Model?.Reload();
        MonitorPlugin.SaveConfig(_obj, _config);
    }

    partial void OnMaxChanged(float value)
    {
        if (_load)
        {
            return;
        }
        var item = _config.Items[Index];
        item.Max = value;
        Model?.Reload();
        MonitorPlugin.SaveConfig(_obj, _config);
    }

    partial void OnMinChanged(float value)
    {
        if (_load)
        {
            return;
        }
        var item = _config.Items[Index];
        item.Min = value;
        Model?.Reload();
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
        Model?.SensorReset();
        MonitorPlugin.SaveConfig(_obj, _config);
    }

    partial void OnFormatChanged(string value)
    {
        if (_load)
        {
            return;
        }
        try
        {
            var temp = string.Format(value, 0.0f);
            FmtError = false;
        }
        catch
        {
            FmtError = true;
            return;
        }
        var item = _config.Items[Index];
        item.Format = value;
        Model?.FormatReset();
        MonitorPlugin.SaveConfig(_obj, _config);
    }

    partial void OnBarBackColorChanged(Color value)
    {
        if (_load)
        {
            return;
        }
        var item = _config.Items[Index];
        item.Color4 = value.ToString();
        Model?.Reload();
        MonitorPlugin.SaveConfig(_obj, _config);
    }

    partial void OnBackColorChanged(Color value)
    {
        if (_load)
        {
            return;
        }
        var item = _config.Items[Index];
        item.Color1 = value.ToString();
        Model?.Reload();
        MonitorPlugin.SaveConfig(_obj, _config);
    }

    partial void OnFontColorChanged(Color value)
    {
        if (_load)
        {
            return;
        }
        var item = _config.Items[Index];
        item.Color2 = value.ToString();
        Model?.Reload();
        MonitorPlugin.SaveConfig(_obj, _config);
    }

    partial void OnBarColorChanged(Color value)
    {
        if (_load)
        {
            return;
        }
        var item = _config.Items[Index];
        item.Color3 = value.ToString();
        Model?.Reload();
        MonitorPlugin.SaveConfig(_obj, _config);
    }

    partial void OnFontSizeChanged(int value)
    {
        if (_load)
        {
            return;
        }
        var item = _config.Items[Index];
        item.FontSize = value;
        Model?.Reload();
        MonitorPlugin.SaveConfig(_obj, _config);
    }

    partial void OnBorderSizeChanged(int value)
    {
        if (_load)
        {
            return;
        }
        var item = _config.Items[Index];
        item.BorderSize = value;
        Model?.Reload();
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
        BorderSize = item.BorderSize;
        FontSize = item.FontSize;
        Format = item.Format;
        Max = item.Max;
        Min = item.Min;

        if (Color.TryParse(item.Color1, out var color))
        {
            BackColor = color;
        }
        else
        {
            BackColor = Colors.Black;
        }
        if (Color.TryParse(item.Color2, out color))
        {
            FontColor = color;
        }
        else
        {
            FontColor = Colors.White;
        }
        if (Color.TryParse(item.Color3, out color))
        {
            BarColor = color;
        }
        else
        {
            BarColor = Colors.White;
        }
        if (Color.TryParse(item.Color4, out color))
        {
            BarBackColor = color;
        }
        else
        {
            BarBackColor = Colors.White;
        }

        SelectItem = Sensors.FirstOrDefault(item1 => item1.Sensor.Identifier.ToString() == item.Sensor);

        DisplaySetting(DisplayType);

        _load = false;
        EnableItem = true;

        Model = new(item)
        {
            Demo = true
        };
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
                DisplayFmt = true;
                DisplayColor = true;
                DisplayBorder = true;
                DisplayBarColor = false;
                DisplayMaxMin = false;
                break;
            case MonitorDisplayType.ProgressBar1:
            case MonitorDisplayType.ProgressBar2:
            case MonitorDisplayType.ProgressBar3:
            case MonitorDisplayType.ProgressBar4:
            case MonitorDisplayType.ProgressBar5:
                DisplaySize = true;
                DisplayFontSize = true;
                DisplayFmt = true;
                DisplayColor = true;
                DisplayBorder = true;
                DisplayBarColor = true;
                DisplayMaxMin = true;
                break;
        }
    }

    private void Update()
    {
        foreach (var item in Sensors)
        {
            item.Update();
        }

        Model?.Update();
    }

    public void Stop()
    {
        _run = false;
    }
}
