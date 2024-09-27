using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorDesktop.WeatherPlugin;

public record WeatherConfigObj
{
    public bool AutoUpdate { get; set; }
    public int UpdateTime { get; set; }
}
