using System.Collections.ObjectModel;
using Avalonia.Controls;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;
using ColorDesktop.CoreLib;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ColorDesktop.Live2DPlugin;

public partial class Live2DInstanceSettingModel : ObservableObject
{
    public partial class ItemNameModel : ObservableObject
    {
        [ObservableProperty]
        private string _name;
    }

    public ObservableCollection<ItemNameModel> Items { get; set; } = [];

    [ObservableProperty]
    private int _width;
    [ObservableProperty]
    private int _height;

    [ObservableProperty]
    private bool _lowFps;
    [ObservableProperty]
    private bool _displayFps;

    [ObservableProperty]
    private int _index = -1;
    [ObservableProperty]
    private bool _enableItem;

    [ObservableProperty]
    private string _name;
    [ObservableProperty]
    private string _local;

    [ObservableProperty]
    private float _x;
    [ObservableProperty]
    private float _y;
    [ObservableProperty]
    private float _scale;

    private readonly InstanceDataObj _obj;
    private readonly Live2DInstanceObj _config;

    private bool _load;

    public Live2DInstanceSettingModel(InstanceDataObj obj)
    {
        _obj = obj;
        _config = Live2DPlugin.GetConfig(obj);

        _width = _config.Width;
        _height = _config.Height;
        _lowFps = _config.LowFps;
        _displayFps = _config.DisplayFps;

        foreach (var item in _config.Models)
        {
            Items.Add(new()
            {
                Name = item.Name
            });
        }

        if (Items.Count > 0)
        {
            Index = 0;
        }
    }

    partial void OnDisplayFpsChanged(bool value)
    {
        _config.DisplayFps = value;
        Live2DPlugin.SaveConfig(_obj, _config);
    }

    partial void OnLowFpsChanged(bool value)
    {
        _config.LowFps = value;
        Live2DPlugin.SaveConfig(_obj, _config);
    }

    partial void OnHeightChanged(int value)
    {
        _config.Height = value;
        Live2DPlugin.SaveConfig(_obj, _config);
    }

    partial void OnWidthChanged(int value)
    {
        _config.Width = value;
        Live2DPlugin.SaveConfig(_obj, _config);
    }

    partial void OnNameChanged(string value)
    {
        if (_load)
        {
            return;
        }

        var item = _config.Models[Index];
        item.Name = value;
        Items[Index].Name = value;
        Live2DPlugin.SaveConfig(_obj, _config);
    }

    partial void OnLocalChanged(string value)
    {
        if (_load)
        {
            return;
        }

        var item = _config.Models[Index];
        item.Local = value;
        Live2DPlugin.SaveConfig(_obj, _config);

        Live2DPlugin.Update(_obj.UUID);
    }

    partial void OnXChanged(float value)
    {
        if (_load)
        {
            return;
        }

        var item = _config.Models[Index];
        item.X = value;
        Live2DPlugin.SaveConfig(_obj, _config);

        Live2DPlugin.Update(_obj.UUID);
    }

    partial void OnYChanged(float value)
    {
        if (_load)
        {
            return;
        }

        var item = _config.Models[Index];
        item.Y = value;
        Live2DPlugin.SaveConfig(_obj, _config);

        Live2DPlugin.Update(_obj.UUID);
    }

    partial void OnScaleChanged(float value)
    {
        if (_load)
        {
            return;
        }

        var item = _config.Models[Index];
        item.Scale = value;
        Live2DPlugin.SaveConfig(_obj, _config);

        Live2DPlugin.Update(_obj.UUID);
    }

    partial void OnIndexChanged(int value)
    {
        if (value == -1)
        {
            EnableItem = false;
            return;
        }
        _load = true;

        var item = _config.Models[value];
        Name = item.Name;
        Local = item.Local;
        X = item.X;
        Y = item.Y;
        Scale = item.Scale;

        _load = false;
        EnableItem = true;
    }

    [RelayCommand]
    public void NewItem()
    {
        var item = Live2DPlugin.MakeNewItem();
        Items.Add(new()
        {
            Name = item.Name
        });
        _config.Models.Add(item);
        Live2DPlugin.SaveConfig(_obj, _config);
        Index = Items.Count - 1;
    }

    [RelayCommand]
    public void DeleteItem()
    {
        if (Index == -1)
        {
            return;
        }

        _config.Models.RemoveAt(Index);
        Items.RemoveAt(Index);
        Live2DPlugin.SaveConfig(_obj, _config);
        if (Index == Items.Count)
        {
            Index--;
        }
    }

    [RelayCommand]
    public async Task SelectFile(Control? control)
    {
        var top = TopLevel.GetTopLevel(control);
        if (top == null)
        {
            return;
        }
        var res = await CoreHelper.SelectFile(top, LangApi.GetLang("Live2DInstanceSetting.Info1"),
            ["*.model3.json"], LangApi.GetLang("Live2DInstanceSetting.Info2"));
        if (res?.Any() == true && res[0].GetPath() is { } dir)
        {
            Local = dir;
        }
    }
}
