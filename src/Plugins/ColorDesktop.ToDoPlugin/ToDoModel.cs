using System.Collections.ObjectModel;
using System.Reflection.Metadata;
using Avalonia.Media;
using ColorDesktop.Api;
using ColorDesktop.ToDoPlugin.Net;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ColorDesktop.ToDoPlugin;

public partial class ToDoModel : ObservableObject, ISelect<ToDoItemModel>
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
                _taskList.Add(item1.ID, new(item1));
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
}
