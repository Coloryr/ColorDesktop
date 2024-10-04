using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.PGLauncherPlugin.ColorMC;

public partial class GameItemModel : ObservableObject
{
    public string Name { get; init; }

    public bool DisplayIcon { get; init; }
    public bool DisplayName { get; init; }

    public Bitmap Icon { get; init; }
    public IBrush TextColor { get; init; }
    public IBrush BackColor { get; init; }

    [ObservableProperty]
    private bool _isOver;

    private readonly GameSettingObj _obj;

    public GameItemModel(GameSettingObj item, DisplayType display, IBrush text, IBrush back)
    {
        _obj = item;

        BackColor = back;
        TextColor = text;
        Name = item.Name;

        if (display is DisplayType.Name or DisplayType.NameIcon)
        {
            DisplayName = true;
        }

        if (display is DisplayType.Icon or DisplayType.NameIcon)
        {
            DisplayIcon = true;

            Bitmap? icon = ColorMCUtils.GetIcon(item);
            if (icon == null)
            {
                var assm = Assembly.GetExecutingAssembly();
                using var item1 = assm.GetManifestResourceStream("ColorDesktop.PGLauncherPlugin.ColorMC.Resource.game.png")!;
                Icon = new Bitmap(item1);
            }
            else
            {
                Icon = icon;
            }
        }
    }

    public void Launch()
    {
        ColorMCUtils.Launch(_obj);
    }
}
