﻿using System;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Threading;
using ColorDesktop.Api;
using ColorDesktop.Launcher.Manager;
using ColorDesktop.Launcher.Objs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ColorDesktop.Launcher.ViewModels;

public partial class AppModel : ObservableObject
{
    public class WindowCallCommand(InstanceWindowObj obj) : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            Dispatcher.UIThread.Post(obj.Window.Activate);
        }
    }

    private static NativeMenuItem BuildWindowMenu()
    {
        var menu = new NativeMenuItem();
        var menu1 = new NativeMenu();

        foreach (var item in InstanceManager.RunInstances.Values)
        {
            menu1.Items.Add(new NativeMenuItem()
            {
                Header = item.InstanceData.Nick,
                Command = new WindowCallCommand(item)
            });
        }

        menu.IsEnabled = InstanceManager.RunInstances.Values.Count != 0;
        menu.Header = LangApi.GetLang("ToolSetting.Text5");
        menu.Menu = menu1;

        return menu;
    }

    public NativeMenu Menus { get; init; } = [];

    public AppModel()
    {
        BuildMenu();
    }

    public void Update()
    {
        BuildMenu();
    }

    private void BuildMenu()
    {
        Menus.Items.Clear();
        Menus.Items.Add(BuildWindowMenu());
        Menus.Items.Add(new NativeMenuItem()
        {
            Header = LangApi.GetLang("ToolSetting.Text1"),
            Command = ShowWindowCommand
        });
        Menus.Items.Add(new NativeMenuItem()
        {
            Header = LangApi.GetLang("ToolSetting.Text4"),
            Command = MoveWindowCommand
        });
        Menus.Items.Add(new NativeMenuItemSeparator());
        Menus.Items.Add(new NativeMenuItem()
        {
            Header = LangApi.GetLang("ToolSetting.Text3"),
            Command = ExitCommand
        });
    }

    [RelayCommand]
    public void ShowWindow()
    {
        App.ShowMainWindow();
    }

    [RelayCommand]
    public void Exit()
    {
        App.Exit();
    }

    [RelayCommand]
    public void MoveWindow()
    {
        InstanceManager.Move();
    }
}
