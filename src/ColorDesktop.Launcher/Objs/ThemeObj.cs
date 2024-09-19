using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media;

namespace ColorDesktop.Launcher.Objs;

public record ThemeObj
{
    public IBrush WindowSideBG;
    public IBrush WindowSideFont;
    public IBrush WindowSideBGTop;
    public IBrush WindowSideBGSelect;
    public IBrush ButtonOver;
    public IBrush ButtonBorder;
    public IBrush ButtonBG;
    public IBrush FontColor;
    public IBrush MainColor;
}
