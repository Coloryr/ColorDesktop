using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorDesktop.PGLauncherPlugin.ColorMC;

public enum DisplayType
{ 
    Name, Icon, NameIcon
}

public record PGColorMCInstanceObj
{
    public int Width { get; set; }
    public int Height { get; set; }
    public string BackColor { get; set; }
    public string TextColor { get; set; }
    public string? GroupName { get; set; }
    public DisplayType Display { get; set; }
}
