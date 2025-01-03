using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorDesktop.Api.Objs;

public record MarginObj
{
    public MarginObj()
    {

    }

    public MarginObj(MarginObj margin)
    {
        Left = margin.Left;
        Right = margin.Right;
        Top = margin.Top;
        Bottom = margin.Bottom;
    }

    public MarginObj(int left, int right, int top, int bottom)
    {
        Left = left;
        Right = right;
        Top = top;
        Bottom = bottom;
    }
    public MarginObj(int a) : this(a, a, a, a) { }

    public int Left { get; set; }
    public int Right { get; set; }
    public int Top { get; set; }
    public int Bottom { get; set; }
}

