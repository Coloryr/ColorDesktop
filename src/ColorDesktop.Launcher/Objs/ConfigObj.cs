using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorDesktop.Launcher.Objs;

public record ConfigObj
{
    public bool AutoStart { get; set; }
    public List<string> EnablePlugin { get; set; }
    public List<string> EnableInstance { get; set; }
}
