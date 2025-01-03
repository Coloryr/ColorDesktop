using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorDesktop.Launcher.Objs;

public class GroupObj
{
    public string Name { get; set; }
    public string UUID { get; set; }
    public HashSet<string> Instances { get; set; }
    public HashSet<string> Enables { get; set; }
}
