using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorDesktop.ToDoPlugin.Objs;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.ToDoPlugin;

public partial class SubTaskItemModel : ObservableObject
{
    [ObservableProperty]
    private bool _isCheck;

    public string Text => Obj.DisplayName;

    public readonly ToDoTaskListObj.ValueObj.CheckListObj Obj;
    public SubTaskItemModel(ToDoTaskListObj.ValueObj.CheckListObj item)
    {
        Obj = item;
        _isCheck = item.IsChecked;
    }
}

public partial class ToDoTaskItemModel : ObservableObject
{
    [ObservableProperty]
    private bool _isCheck;

    public string Text => Obj.Body.Content;
    public string Title => Obj.Title;
    public bool HaveDueTime => Obj.DueDateTime != null;
    public bool HaveSubTask { get; init; }
    public bool HaveText => !string.IsNullOrWhiteSpace(Text);

    public ObservableCollection<SubTaskItemModel> SubTasks { get; init; } = [];

    public readonly ToDoTaskListObj.ValueObj Obj;

    public ToDoTaskItemModel(ToDoTaskListObj.ValueObj obj)
    {
        Obj = obj;

        if (obj.ChecklistItems != null)
        {
            HaveSubTask = true;
            foreach (var item in obj.ChecklistItems)
            {
                SubTasks.Add(new(item));
            }
        }

        IsCheck = obj.CompletedDateTime != null;
    }
}
