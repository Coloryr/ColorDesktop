using ColorDesktop.Api.Objs;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.Launcher.UI.Models.Items;

public partial class ChoiseInstanceItemModel : ObservableObject
{
    public string Nick => _obj.Nick;
    public string Plugin => _obj.Plugin;
    public string Comment => _obj.Comment;

    public string UUID => _obj.UUID;

    [ObservableProperty]
    private bool _isCheck;

    private readonly InstanceDataObj _obj;

    public ChoiseInstanceItemModel(InstanceDataObj obj)
    {
        _obj = obj;


    }
}
