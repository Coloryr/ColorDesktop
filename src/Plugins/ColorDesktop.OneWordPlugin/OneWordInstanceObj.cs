using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorDesktop.OneWordPlugin;

public record OneWordInstanceObj
{
    public int Size { get; set; }
    public string TextColor { get; set; }
    public string BackColor { get; set; }
    public int Width { get; set; }
}
