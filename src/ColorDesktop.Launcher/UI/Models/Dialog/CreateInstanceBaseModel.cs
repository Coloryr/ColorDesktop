﻿using ColorDesktop.Api;
using ColorDesktop.Api.Objs;
using ColorDesktop.Launcher.Helper;
using ColorDesktop.Launcher.UI.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;

namespace ColorDesktop.Launcher.UI.Models.Dialog;

public abstract partial class CreateInstanceBaseModel : ObservableObject
{
    [ObservableProperty]
    private PosEnum _pos;

    [ObservableProperty]
    private string _nick;
    [ObservableProperty]
    private string _comment;

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
    [ObservableProperty]
    private bool _mouseThrough;

    [ObservableProperty]
    private WindowTransparencyType _tran;

    public string[] TranTypes { get; init; } = LangHelper.GetWindowTranTypeLang();

    private readonly InstanceDataObj _obj;

    public CreateInstanceBaseModel(InstanceDataObj obj)
    {
        _obj = obj;
        _nick = obj.Nick;
        _display = obj.Display;
        _pos = obj.Pos;
        _topModel = obj.TopModel;
        _tran = obj.Tran;
        _comment = obj.Comment;
        _mouseThrough = obj.MouseThrough;
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
        _obj.Tran = Tran;
        _obj.Comment = Comment;
        _obj.MouseThrough = MouseThrough;
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
