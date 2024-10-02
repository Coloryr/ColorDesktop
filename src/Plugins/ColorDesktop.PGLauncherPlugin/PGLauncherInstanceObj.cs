using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorDesktop.Api;

namespace ColorDesktop.PGLauncherPlugin;

public enum DisplayType
{
    Text, Img, Icon, TextImg, TextIcon
}

public enum PanelType
{
    Stack, Wrap, Panel
}

public record PGItemObj
{
    public string Name { get; set; }
    public DisplayType Display { get; set; }
    public int Size { get; set; }
    public string Local { get; set; }
    public string Arg { get; set; }
    public string Icon { get; set; }
    public MarginObj Margin { get; set; }
    public string BackColor { get; set; }
    public string TextColor { get; set; }
    public int BorderSize { get; set; }
    public int TextSize { get; set; }
}

public record PGLauncherInstanceObj
{
    public int Height { get; set; }
    public int Width { get; set; }
    public List<PGItemObj> Items { get; set; }
    public PanelType PanelType { get; set; }
}
