using ColorDesktop.ToDoPlugin.Objs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ColorDesktop.ToDoPlugin;

public partial class ToDoCheckItemItemModel : ObservableObject
{
    public const string EditName = "Edit";

    [ObservableProperty]
    private bool _isCheck;
    [ObservableProperty]
    private bool _isOver;
    [ObservableProperty]
    private bool _isEdit;

    [ObservableProperty]
    public string _text;

    private readonly ToDoTaskListObj.ValueObj1.CheckListObj _obj;
    private readonly ToDoTaskItemModel _top;

    public ToDoCheckItemItemModel(ToDoTaskItemModel top, ToDoTaskListObj.ValueObj1.CheckListObj item)
    {
        _top = top;
        _obj = item;
        _isCheck = item.IsChecked;
        _text = _obj.DisplayName;
    }

    partial void OnIsCheckChanged(bool value)
    {
        _top.EditCheckItem(_obj.Id, value);
    }

    [RelayCommand]
    public void DeleteCheckItem()
    {
        _top.DeleteCheckItem(_obj.Id);
    }

    [RelayCommand]
    public void EditCheckItem()
    {
        if (IsEdit)
        {
            return;
        }

        IsEdit = true;
        OnPropertyChanged(EditName);
    }

    public void EditEnd()
    {
        if (!IsEdit)
        {
            return;
        }

        IsEdit = false;

        if (Text != _obj.DisplayName)
        {
            _top.EditCheckItem(_obj.Id, text: Text);
        }
    }

    public void Cancel()
    {
        IsEdit = false;
        Text = _obj.DisplayName;
    }
}
