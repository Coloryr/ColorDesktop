using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using ColorDesktop.Api;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ColorDesktop.PGLauncherPlugin;

public partial class PGLauncherInstanceSettingModel : ObservableObject
{
    public partial class ItemNameModel : ObservableObject
    {
        [ObservableProperty]
        private string _name;
    }

    public ObservableCollection<ItemNameModel> Items { get; init; } = [];

    [ObservableProperty]
    public PGItemModel _model;

    public string[] PanelTypeName { get; init; } = 
    [
        LangApi.GetLang("PGPanelType.Type1"), 
        LangApi.GetLang("PGPanelType.Type2"), 
        LangApi.GetLang("PGPanelType.Type3")
    ];
    public string[] DisplayTypeName { get; init; } = 
    [
        LangApi.GetLang("PGDisplayType.Type1"), 
        LangApi.GetLang("PGDisplayType.Type2"), 
        LangApi.GetLang("PGDisplayType.Type3"), 
        LangApi.GetLang("PGDisplayType.Type4"), 
        LangApi.GetLang("PGDisplayType.Type5")
    ];

    [ObservableProperty]
    private int _index = -1;

    [ObservableProperty]
    private int _width;
    [ObservableProperty]
    private int _height;
    [ObservableProperty]
    private bool _autoSize;

    [ObservableProperty]
    private PanelType _type;

    [ObservableProperty]
    private string _name;
    [ObservableProperty]
    private string _local;
    [ObservableProperty]
    private string _arg;
    [ObservableProperty]
    private string _img;
    [ObservableProperty]
    private int _size;
    [ObservableProperty]
    private int _border;
    [ObservableProperty]
    private Color _backColor;
    [ObservableProperty]
    private Color _textColor;
    [ObservableProperty]
    private DisplayType _display;
    [ObservableProperty]
    private int _left;
    [ObservableProperty]
    private int _top;
    [ObservableProperty]
    private int _right;
    [ObservableProperty]
    private int _bottom;
    [ObservableProperty]
    private int _textSize;

    [ObservableProperty]
    private bool _enableImg;
    [ObservableProperty]
    private bool _enableItem;
    [ObservableProperty]
    private bool _admin;
    [ObservableProperty]
    private bool _enableFontSize;

    private bool _load;

    private readonly InstanceDataObj _obj;
    private readonly PGLauncherInstanceObj _config;

    public PGLauncherInstanceSettingModel(InstanceDataObj obj)
    {
        _obj = obj;
        _config = PGLauncherPlugin.GetConfig(obj);

        _config.Items ??= [];

        _width = _config.Width;
        _height = _config.Height;
        _type = _config.PanelType;
        _autoSize = _config.AutoSize;

        foreach (var item in _config.Items)
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

    partial void OnAutoSizeChanged(bool value)
    {
        _config.AutoSize = value;
        PGLauncherPlugin.SaveConfig(_obj, _config);
    }

    partial void OnTypeChanged(PanelType value)
    {
        _config.PanelType = value;
        PGLauncherPlugin.SaveConfig(_obj, _config);
    }

    partial void OnWidthChanged(int value)
    {
        _config.Width = value;
        PGLauncherPlugin.SaveConfig(_obj, _config);
    }

    partial void OnHeightChanged(int value)
    {
        _config.Height = value;
        PGLauncherPlugin.SaveConfig(_obj, _config);
    }

    partial void OnDisplayChanged(DisplayType value)
    {
        EnableImg = value is DisplayType.Img or DisplayType.TextImg;
        EnableFontSize = value is DisplayType.Text or DisplayType.TextIcon or DisplayType.TextImg;
        if (_load)
        {
            return;
        }
        var item = _config.Items[Index];
        item.Display = value;
        ModelLoad(item);
        PGLauncherPlugin.SaveConfig(_obj, _config);
    }

    partial void OnArgChanged(string value)
    {
        if (_load)
        {
            return;
        }
        var item = _config.Items[Index];
        item.Arg = value;
        PGLauncherPlugin.SaveConfig(_obj, _config);
    }

    partial void OnLocalChanged(string value)
    {
        if (_load)
        {
            return;
        }
        var item = _config.Items[Index];
        item.Local = value;
        PGLauncherPlugin.SaveConfig(_obj, _config);
    }

    partial void OnNameChanged(string value)
    {
        if (_load)
        {
            return;
        }
        var item = _config.Items[Index];
        Items[Index].Name = value;
        item.Name = value;
        PGLauncherPlugin.SaveConfig(_obj, _config);
    }

    partial void OnImgChanged(string value)
    {
        if (_load)
        {
            return;
        }
        var item = _config.Items[Index];
        item.Icon = value;
        ModelLoad(item);
        PGLauncherPlugin.SaveConfig(_obj, _config);
    }

    partial void OnSizeChanged(int value)
    {
        if (_load)
        {
            return;
        }
        var item = _config.Items[Index];
        item.Size = value;
        Model.Size = value;
        PGLauncherPlugin.SaveConfig(_obj, _config);
    }

    partial void OnLeftChanged(int value)
    {
        if (_load)
        {
            return;
        }
        var item = _config.Items[Index];
        item.Margin.Left = value;
        PGLauncherPlugin.SaveConfig(_obj, _config);
    }

    partial void OnTopChanged(int value)
    {
        if (_load)
        {
            return;
        }
        var item = _config.Items[Index];
        item.Margin.Top = value;
        PGLauncherPlugin.SaveConfig(_obj, _config);
    }

    partial void OnRightChanged(int value)
    {
        if (_load)
        {
            return;
        }
        var item = _config.Items[Index];
        item.Margin.Right = value;
        PGLauncherPlugin.SaveConfig(_obj, _config);
    }

    partial void OnBottomChanged(int value)
    {
        if (_load)
        {
            return;
        }
        var item = _config.Items[Index];
        item.Margin.Bottom = value;
        PGLauncherPlugin.SaveConfig(_obj, _config);
    }

    partial void OnBorderChanged(int value)
    {
        if (_load)
        {
            return;
        }
        var item = _config.Items[Index];
        item.BorderSize = value;
        Model.Border = new(value);
        PGLauncherPlugin.SaveConfig(_obj, _config);
    }

    partial void OnBackColorChanged(Color value)
    {
        if (_load)
        {
            return;
        }
        var item = _config.Items[Index];
        item.BackColor = value.ToString();
        Model.BackColor = new SolidColorBrush(value, 1);
        PGLauncherPlugin.SaveConfig(_obj, _config);
    }

    partial void OnTextColorChanged(Color value)
    {
        if (_load)
        {
            return;
        }
        var item = _config.Items[Index];
        item.TextColor = value.ToString();
        Model.TextColor = new SolidColorBrush(value, 1);
        PGLauncherPlugin.SaveConfig(_obj, _config);
    }

    partial void OnTextSizeChanged(int value)
    {
        if (_load)
        {
            return;
        }
        var item = _config.Items[Index];
        item.TextSize = value;
        Model.TextSize = value;
        PGLauncherPlugin.SaveConfig(_obj, _config);
    }

    partial void OnAdminChanged(bool value)
    {
        if (_load)
        {
            return;
        }
        var item = _config.Items[Index];
        item.Admin = value;
        PGLauncherPlugin.SaveConfig(_obj, _config);
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
        Local = item.Local;
        Arg = item.Arg;
        Display = item.Display;
        Size = item.Size;
        Img = item.Icon;
        Left = item.Margin.Left;
        Top = item.Margin.Top;
        Right = item.Margin.Right;
        Bottom = item.Margin.Bottom;
        Border = item.BorderSize;
        TextSize = item.TextSize;
        Admin = item.Admin;
        if (Color.TryParse(item.BackColor, out var color))
        {
            BackColor = color;
        }
        else
        {
            BackColor = Colors.Black;
        }
        if (Color.TryParse(item.TextColor, out color))
        {
            TextColor = color;
        }
        else
        {
            TextColor = Colors.White;
        }

        EnableImg = item.Display is DisplayType.Img or DisplayType.TextImg;
        EnableFontSize = item.Display is DisplayType.Text or DisplayType.TextIcon or DisplayType.TextImg;

        _load = false;
        EnableItem = true;
        ModelLoad(item);
    }

    [RelayCommand]
    public void NewItem()
    {
        var item = PGLauncherPlugin.MakeNewItem();
        Items.Add(new()
        { 
            Name = item.Name
        });
        _config.Items.Add(item);
        PGLauncherPlugin.SaveConfig(_obj, _config);
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
        PGLauncherPlugin.SaveConfig(_obj, _config);
        if (Index == Items.Count)
        {
            Index--;
        }
    }

    [RelayCommand]
    public async Task SelectFile(Control? control)
    {
        var file = await SystemUtils.SelectFile(TopLevel.GetTopLevel(control),
            "选择程序文件", SystemInfo.Os == OsType.Windows ? ["*.exe"] : [], "程序");
        if (file == null || file.Count == 0)
        {
            return;
        }

        var item = file[0].GetPath();
        if (item == null || !SystemUtils.IsExecutable(item))
        {
            return;
        }

        Local = item;
    }

    [RelayCommand]
    public async Task SelectIcon(Control? control)
    {
        var file = await SystemUtils.SelectFile(TopLevel.GetTopLevel(control),
            "选择图片文件", ["*.png", "*.jpg", "*.bmp"], "图片");
        if (file == null || file.Count == 0)
        {
            return;
        }

        var item = file[0].GetPath();
        if (!File.Exists(item))
        {
            return;
        }

        Img = item;
    }

    private void ModelLoad(PGItemObj item)
    {
        Model = new(item)
        { 
            Demo = true
        };
    }
}
