using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorDesktop.MinecraftMotdPlugin;

public record MotdInstanceObj
{
    public string IP { get; set; }
    public ushort? Port { get; set; }
}
