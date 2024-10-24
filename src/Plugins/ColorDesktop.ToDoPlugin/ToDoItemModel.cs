using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorDesktop.ToDoPlugin.Objs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ColorDesktop.ToDoPlugin;

public partial class ToDoItemModel(ISelect<ToDoItemModel> top, ToDoListObj.ValueObj obj) : ObservableObject
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
}
