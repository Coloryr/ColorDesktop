using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorDesktop.BmPlugin;

public enum SkinType
{ 
    Skin1, Skin2
}

public record BmInstanceObj
{
    public SkinType Skin { get; set; }
}
