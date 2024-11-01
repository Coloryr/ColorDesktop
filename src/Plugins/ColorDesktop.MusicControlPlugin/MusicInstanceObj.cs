using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorDesktop.MusicControlPlugin;

public enum SkinType
{ 
    Skin1
}

public record MusicInstanceObj
{
    public SkinType Skin { get; set; }
    public int Width { get; set; }
}
