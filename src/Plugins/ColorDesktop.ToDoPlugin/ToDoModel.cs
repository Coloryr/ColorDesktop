﻿using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Avalonia.Media;
using ColorDesktop.Api;
using ColorDesktop.ToDoPlugin.Dialog;
using ColorDesktop.ToDoPlugin.Net;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;

namespace ColorDesktop.ToDoPlugin;

public partial class ToDoModel(string uuid) : ObservableObject, ISelect<ToDoItemModel>
{
    [ObservableProperty]
    private bool _loginFail;
    [ObservableProperty]
    private bool _loadFail;
    [ObservableProperty]
    private bool _isLoad;

    [ObservableProperty]
    private IBrush _backColor;
    [ObservableProperty]
    private IBrush _textColor;

    [ObservableProperty]
    private bool _displaySide;
    [ObservableProperty]
    private bool _displayButton;

    [ObservableProperty]
    private ToDoItemModel? _selectList;

    public ObservableCollection<ToDoItemModel> TodoList { get; init; } = [];

    public ObservableCollection<ToDoTaskItemModel> TaskList { get; init; } = [];

    private readonly Dictionary<string, ToDoTaskItemModel> _taskList = [];
    private readonly Dictionary<string, List<string>> _todolist = [];

    private InstanceDataObj _obj;
    private ToDoInstanceObj _config;

    public string UUID { get; init; } = "ToDo" + uuid;

    public async void Update(InstanceDataObj obj, ToDoInstanceObj config)
    {
        _obj = obj;
        _config = config;

        BackColor = string.IsNullOrWhiteSpace(_config.BackColor)
            ? Brushes.Black : Brush.Parse(_config.BackColor);
        TextColor = string.IsNullOrWhiteSpace(_config.TextColor)
            ? Brushes.White : Brush.Parse(_config.TextColor);

        await Reload();
    }

    [RelayCommand]
    public void OpenSide()
    {
        DisplaySide = true;
    }

    [RelayCommand]
    public void CloseSide()
    {
        DisplaySide = false;
    }

    [RelayCommand]
    public void Add()
    {

    }

    [RelayCommand]
    public async Task Reload()
    {
        IsLoad = true;
        if (string.IsNullOrWhiteSpace(_config.Token))
        {
            var res = await Refresh();
            if (!res)
            {
                SetLoginFail();
                IsLoad = false;
                return;
            }
        }

        var data = await ToDoApi.GetLists(_config.Token);
        if (!data.Item1)
        {
            var res = await Refresh();
            if (!res)
            {
                SetLoginFail();
                IsLoad = false;
                return;
            }

            data = await ToDoApi.GetLists(_config.Token);
            if (!data.Item1)
            {
                SetLoginFail();
                IsLoad = false;
                return;
            }
        }

        if (data.Item2 == null)
        {
            LoadFail = true;
            IsLoad = false;
            return;
        }

        TodoList.Clear();
        _todolist.Clear();
        foreach (var item in data.Item2.Value)
        {
            TodoList.Add(new(this, item));
            _todolist.Add(item.Id, []);
        }

        await LoadItem();

        if (TodoList.Count > 0)
        {
            Select(TodoList[0]);
        }

        IsLoad = false;
    }

    private async Task LoadItem()
    {
        _taskList.Clear();

        foreach (var item in _todolist)
        {
            var res = await ToDoApi.GetTaskLists(_config.Token, item.Key);
            if (!res.Item1 || res.Item2 == null)
            {
                LoadFail = true;
                return;
            }

            foreach (var item1 in res.Item2.Value)
            {
                item.Value.Add(item1.ID);
                var item2 = new ToDoTaskItemModel(this, item.Key, UUID);
                item2.Update(item1);
                _taskList.Add(item1.ID, item2);
            }
        }
    }

    private void LoadTask()
    {
        if (SelectList == null)
        {
            return;
        }
        TaskList.Clear();
        if (_todolist.TryGetValue(SelectList.Id, out var list))
        {
            foreach (var item in list)
            {
                if (_taskList.TryGetValue(item, out var task))
                {
                    TaskList.Add(task);
                }
            }
        }
    }

    private void SetLoginFail()
    {
        LoginFail = true;
    }

    private async Task<bool> Refresh()
    {
        var res = await OAuth.RefreshTokenAsync(_config.RefreshToken);
        if (res.State == LoginState.Done)
        {
            _config.Token = res.Obj!.AccessToken;
            _config.RefreshToken = res.Obj.RefreshToken;

            ToDoPlugin.SaveConfig(_obj, _config);

            return true;
        }

        return false;
    }

    public void Select(ToDoItemModel item)
    {
        if (SelectList != null)
        {
            SelectList.IsCheck = false;
        }

        SelectList = item;
        SelectList.IsCheck = true;

        LoadTask();

        CloseSide();
    }

    public async void CreateCheckItem(string list, string task, string step)
    {
        var res = await ToDoApi.CreateTaskCheckList(_config.Token, list, task, step);
        if (!res)
        {
            res = await Refresh();
            if (!res)
            {
                SetLoginFail();
                IsLoad = false;
                return;
            }

            res = await ToDoApi.CreateTaskCheckList(_config.Token, list, task, step);
            if (!res)
            {
                SetLoginFail();
                IsLoad = false;
                return;
            }
        }

        ReloadTask(list, task);
    }

    public async void DeleteCheckItem(string listId, string task, string id)
    {
        var res1 = await DialogHost.Show(new ChoiseModel(UUID)
        { 
            Text = LangApi.GetLang("ToDoControl.Info1")
        }, UUID);
        if (res1 is not true)
        {
            return;
        }

        var res = await ToDoApi.DeleteTaskCheckList(_config.Token, listId, task, id);
        if (!res)
        {
            res = await Refresh();
            if (!res)
            {
                SetLoginFail();
                IsLoad = false;
                return;
            }

            res = await ToDoApi.DeleteTaskCheckList(_config.Token, listId, task, id);
            if (!res)
            {
                SetLoginFail();
                IsLoad = false;
                return;
            }
        }

        ReloadTask(listId, task);
    }

    public async void EditCheckItem(string listId, string task, string id, bool? isCheck, string? text)
    {
        var res = await ToDoApi.EditCheckItem(_config.Token, listId, task, id, isCheck, text);
        if (!res)
        {
            res = await Refresh();
            if (!res)
            {
                SetLoginFail();
                IsLoad = false;
                return;
            }

            res = await ToDoApi.EditCheckItem(_config.Token, listId, task, id, isCheck, text);
            if (!res)
            {
                SetLoginFail();
                IsLoad = false;
                return;
            }
        }

        ReloadTask(listId, task);
    }

    public async void DeleteTaskItem(string listId, string task)
    {
        var res1 = await DialogHost.Show(new ChoiseModel(UUID)
        {
            Text = LangApi.GetLang("ToDoControl.Info2")
        }, UUID);
        if (res1 is not true)
        {
            return;
        }

        var res = await ToDoApi.DeleteTask(_config.Token, listId, task);
        if (!res)
        {
            res = await Refresh();
            if (!res)
            {
                SetLoginFail();
                IsLoad = false;
                return;
            }

            res = await ToDoApi.DeleteTask(_config.Token, listId, task);
            if (!res)
            {
                SetLoginFail();
                IsLoad = false;
                return;
            }
        }

        LoadTask(); 
    }

    public async void EditTaskItem(string listId, string task, string? text = null, bool? isCheck = null, DateTime? time = null, string? body = null)
    {
        var res = await ToDoApi.EditTaskItem(_config.Token, listId, task, text, isCheck, time, body);
        if (!res)
        {
            res = await Refresh();
            if (!res)
            {
                SetLoginFail();
                IsLoad = false;
                return;
            }

            res = await ToDoApi.EditTaskItem(_config.Token, listId, task, text, isCheck, time, body);
            if (!res)
            {
                SetLoginFail();
                IsLoad = false;
                return;
            }
        }

        ReloadTask(listId, task);
    }

    private async void ReloadTask(string list, string task)
    {
        var res1 = await ToDoApi.GetTask(_config.Token, list, task);
        if (!res1.Item1)
        {
            SetLoginFail();
            IsLoad = false;
            return;
        }

        if (_taskList.TryGetValue(task, out var model))
        {
            model.Update(res1.Item2!);
        }
    }
}
