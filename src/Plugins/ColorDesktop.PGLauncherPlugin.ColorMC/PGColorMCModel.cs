using System.Collections.ObjectModel;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.PGLauncherPlugin.ColorMC;

public partial class PGColorMCModel : ObservableObject
{
    public ObservableCollection<GameItemModel> Games { get; init; } = [];

    [ObservableProperty]
    private int _width;
    [ObservableProperty]
    private int _height;

    [ObservableProperty]
    private IBrush _backColor;
    [ObservableProperty]
    private IBrush _textColor;

    [ObservableProperty]
    private bool _haveColorMC;

    [ObservableProperty]
    private string _group;

    public void Update(PGColorMCInstanceObj obj)
    {
        Width = obj.Width;
        Height = obj.Height;

        BackColor = Brush.Parse(obj.BackColor);
        TextColor = Brush.Parse(obj.TextColor);

        Games.Clear();

        var games = ColorMCUtils.GetGames();
        if (games == null)
        {
            HaveColorMC = false;
            return;
        }

        var group = obj.GroupName;
        if (string.IsNullOrWhiteSpace(group))
        {
            Group = "默认分组";

            foreach (var item in games.Where(item => item.GroupName == "" || item.GroupName == null))
            {
                Games.Add(new(item, obj.Display, TextColor, BackColor));
            }
        }
        else
        {
            Group = group;

            foreach (var item in games.Where(item => item.GroupName == group))
            {
                Games.Add(new(item, obj.Display, TextColor, BackColor));
            }
        }

        HaveColorMC = true;
    }
}
