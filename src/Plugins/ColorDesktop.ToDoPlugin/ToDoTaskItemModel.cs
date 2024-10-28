﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorDesktop.ToDoPlugin.Dialog;
using ColorDesktop.ToDoPlugin.Net;
using ColorDesktop.ToDoPlugin.Objs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;

namespace ColorDesktop.ToDoPlugin;

public enum DayOffTask
{ 
    None, Today, Day1, Day2, TimeOut
}

public partial class ToDoTaskItemModel(ToDoModel top, string id, string uuid) : ObservableObject
{
    public const string StepName = "StepInput";
    public const string TitleName = "TitleEdit";
    public const string BodyName = "BodyEdit";

    [ObservableProperty]
    private bool _isCheck;
    [ObservableProperty]
    private bool _isOver;
    [ObservableProperty]
    private bool _isNotifyDisplay;
    [ObservableProperty]
    private bool _isNotify;
    [ObservableProperty]
    private bool _isEdit;

    [ObservableProperty]
    private bool _haveDueTime;
    [ObservableProperty]
    private string _dueTime;

    [ObservableProperty]
    private bool _newStep;
    [ObservableProperty]
    private string _step;

    [ObservableProperty]
    private DayOffTask _dayOff;
    [ObservableProperty]
    private DateTime _time;

    [ObservableProperty]
    public string _text;
    [ObservableProperty]
    public string _title;

    [ObservableProperty]
    public bool _haveSubTask;
    [ObservableProperty]
    public bool _haveText;
    [ObservableProperty]
    public bool _editTitle;
    [ObservableProperty]
    public bool _editBody;

    public ObservableCollection<ToDoCheckItemItemModel> SubTasks { get; init; } = [];

    private ToDoTaskListObj.ValueObj _obj;
    private readonly string _listId = id;

    private bool _pointer;
    private bool _isLoad;

    partial void OnIsCheckChanged(bool value)
    {
        if (value)
        {
            IsOver = false;
        }
        else
        {
            UpdateTime();
            if (_pointer)
            {
                IsOver = true;
            }
        }

        if (_isLoad)
        {
            return;
        }

        IsEdit = true;
        top.EditTaskItem(_listId, _obj.ID, isCheck: value);
        IsEdit = false;
    }

    partial void OnIsOverChanged(bool value)
    {
        IsNotifyDisplay = value || IsNotify;

        foreach (var item in SubTasks)
        {
            item.IsOver = value;
        }
    }

    partial void OnIsNotifyChanged(bool value)
    {
        IsNotifyDisplay = value || IsOver;
    }

    [RelayCommand]
    public void Add()
    {
        if (NewStep)
        {
            return;
        }

        Step = "";
        NewStep = true;

        OnPropertyChanged(StepName);
    }

    [RelayCommand]
    public void EditTaskItem()
    {
        if (EditTitle)
        {
            return;
        }

        EditTitle = true;
        OnPropertyChanged(TitleName);
    }

    [RelayCommand]
    public void DeleteTaskItem()
    {
        IsEdit = true;
        top.DeleteTaskItem(_listId, _obj.ID);
    }

    [RelayCommand]
    public async Task SelectTime()
    {
        var model = new ChoiseTimeModel(uuid)
        {
            Time = new DateTimeOffset(Time)
        };
        var res = await DialogHost.Show(model, uuid);
        if (res is not true)
        {
            return;
        }

        IsEdit = true;
        top.EditTaskItem(_listId, _obj.ID, time: model.Time.DateTime);
        IsEdit = false;
    }

    [RelayCommand]
    public void EditBodyText()
    {
        if (EditTitle)
        {
            return;
        }
        EditTitle = true;
        OnPropertyChanged(BodyName);
    }

    public void BodyEnd()
    {
        if (!EditTitle)
        {
            return;
        }
        EditTitle = false;

        if (Text != _obj.Body.Content)
        {
            IsEdit = true;
            top.EditTaskItem(_listId, _obj.ID, body: Text);
            IsEdit = false;
        }
    }

    public void BodyCancel()
    {
        EditTitle = false;
        Text = _obj.Body.Content;
    }

    public void TitleEnd()
    {
        if (!EditTitle)
        {
            return;
        }
        EditTitle = false;

        if (Title != _obj.Title)
        {
            IsEdit = true;
            top.EditTaskItem(_listId, _obj.ID, Title);
            IsEdit = false;
        }
    }

    public void TitleCancel()
    {
        EditTitle = false;
        Title = _obj.Title;
    }

    public void DeleteCheckItem(string id)
    {
        IsEdit = true;
        top.DeleteCheckItem(_listId, _obj.ID, id);
        IsEdit = false;
    }

    public void Update(ToDoTaskListObj.ValueObj obj)
    {
        _obj = obj;

        _isLoad = true;
        SubTasks.Clear();
        if (obj.ChecklistItems != null)
        {
            HaveSubTask = true;
            foreach (var item in obj.ChecklistItems)
            {
                SubTasks.Add(new(this, item)
                { 
                    IsOver = IsOver
                });
            }
        }
        else
        {
            HaveSubTask = false;
        }

        Text = obj.Body.Content;
        Title = obj.Title;
        HaveText = !string.IsNullOrWhiteSpace(Text);
        IsCheck = obj.CompletedDateTime != null;

        UpdateTime();
        _isLoad = false;
    }

    public void NewStepCancel()
    {
        NewStep = false;
        Step = "";
    }

    public void NewStepEnd()
    {
        if (!NewStep)
        {
            return;
        }
        NewStep = false;
        if (string.IsNullOrWhiteSpace(Step))
        {
            return;
        }

        top.CreateCheckItem(_listId, _obj.ID, Step);
    }

    public void PointerOver(bool enter)
    {
        _pointer = enter;

        if (IsCheck)
        {
            return;
        }

        IsOver = enter;
    }

    public void EditCheckItem(string id, bool? isCheck = null, string? text = null)
    {
        top.EditCheckItem(_listId, _obj.ID, id, isCheck, text);
    }

    private void UpdateTime()
    {
        if (_obj.DueDateTime != null)
        {
            var time = DateTime.Now;
            var time1 = new DateTime(time.Year, time.Month, time.Day, 0, 0, 0, DateTimeKind.Local);
            var tiem2 = _obj.DueDateTime.DateTime.ToLocalTime();
            var less = tiem2 - time1;
            if (IsCheck)
            {
                DayOff = DayOffTask.None;
            }
            else
            {
                if (less.Ticks < 0)
                {
                    DayOff = DayOffTask.TimeOut;
                }
                else if (less.TotalDays < 1)
                {
                    DayOff = DayOffTask.Today;
                }
                else if (less.TotalDays < 2)
                {
                    DayOff = DayOffTask.Day1;
                }
                else if (less.TotalDays < 3)
                {
                    DayOff = DayOffTask.Day2;
                }
            }
            HaveDueTime = true;
            DueTime = _obj.DueDateTime.DateTime.ToString("D");
        }
        else
        {
            HaveDueTime = false;
            DueTime = "未设置";
        }
    }
}
