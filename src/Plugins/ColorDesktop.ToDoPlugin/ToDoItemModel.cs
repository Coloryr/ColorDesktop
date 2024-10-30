using ColorDesktop.ToDoPlugin.Dialog;
using ColorDesktop.ToDoPlugin.Objs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;

namespace ColorDesktop.ToDoPlugin;

public partial class ToDoItemModel(string name, ToDoModel top, ToDoListObj.ValueObj obj) : ObservableObject
{
    [ObservableProperty]
    private bool _isCheck;

    public string Name => Obj.DisplayName;

    public string Id => Obj.Id;

    public readonly ToDoListObj.ValueObj Obj = obj;

    [RelayCommand]
    public void Select()
    {
        top.Select(this);
    }

    [RelayCommand]
    public async Task Edit()
    {
        var model = new NewTaskModel(name)
        { 
            Title = Name
        };
        var res1 = await DialogHost.Show(model, name);
        if (res1 is not true)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(model.Title))
        {
            return;
        }

        top.EditTaskList(Id, model.Title);
    }
}
