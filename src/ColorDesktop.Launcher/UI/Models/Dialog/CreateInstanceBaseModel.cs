using ColorDesktop.Api;
using ColorDesktop.Launcher.UI.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;

namespace ColorDesktop.Launcher.UI.Models.Dialog;

public partial class CreateInstanceBaseModel : ObservableObject
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

    [ObservableProperty]
    private bool _haveCancel = true;

    [ObservableProperty]
    private bool _topModel;

    private readonly InstanceDataObj _obj;

    public CreateInstanceBaseModel(InstanceDataObj obj)
    {
        _obj = obj;
        _nick = obj.Nick;
        _display = obj.Display;
        _pos = obj.Pos;
        _topModel = obj.TopModel;
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
        _obj.TopModel = TopModel;
        ToMargin(_obj);

        DialogHost.Close(MainWindow.DialogHostName, true);
    }

    [RelayCommand]
    public void Cancel()
    {
        DialogHost.Close(MainWindow.DialogHostName, false);
    }

    private void ToMargin(InstanceDataObj obj)
    {
        obj.Margin = new(Left, Right, Top, Bottom);
    }

    private void MarginTo(InstanceDataObj obj)
    {
        Left = obj.Margin.Left;
        Top = obj.Margin.Top;
        Right = obj.Margin.Right;
        Bottom = obj.Margin.Bottom;
    }
}
