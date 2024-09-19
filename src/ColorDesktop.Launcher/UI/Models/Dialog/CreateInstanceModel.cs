using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Layout;
using ColorDesktop.Api;
using ColorDesktop.Launcher.Objs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;

namespace ColorDesktop.Launcher.UI.Models.Dialog;

public partial class CreateInstanceModel : ObservableObject
{
    [ObservableProperty]
    private PosEnum _pos;

    [ObservableProperty]
    private string _nick;

    [ObservableProperty]
    private int _left;
    [ObservableProperty]
    private int _top;
    [ObservableProperty]
    private int _right;
    [ObservableProperty]
    private int _bottom;

    [ObservableProperty]
    private int _display;

    [ObservableProperty]
    private int _maxDisplay;

    private readonly InstanceDataObj _obj;

    public CreateInstanceModel(InstanceDataObj obj)
    {
        _obj = obj;
        _nick = obj.Nick;
        _display = obj.Display;
        _pos = obj.Pos;
        MarginTo(obj);
        _maxDisplay = App.MainWindow!.Screens.ScreenCount;
        if (_display == 0 || _display > _maxDisplay)
        {
            _display = 1;
        }
    }

    [RelayCommand]
    public void Done()
    {
        _obj.Pos = Pos;
        _obj.Nick = Nick;
        _obj.Display = Display;
        ToMargin(_obj);

        DialogHost.Close("MainWindow", _obj);
    }

    [RelayCommand]
    public void Cancel()
    {
        DialogHost.Close("MainWindow", null);
    }

    private void ToMargin(InstanceDataObj obj)
    {
        obj.Margin = new(Left, Top, Right, Bottom);
    }

    private void MarginTo(InstanceDataObj obj)
    {
        Left = (int)obj.Margin.Left;
        Top = (int)obj.Margin.Top;
        Right = (int)obj.Margin.Right;
        Bottom = (int)obj.Margin.Bottom;
    }
}
