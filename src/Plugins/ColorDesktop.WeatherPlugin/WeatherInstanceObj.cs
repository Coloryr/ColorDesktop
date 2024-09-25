using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorDesktop.WeatherPlugin;

public record WeatherInstanceObj
{
    public string City { get; set; }
    public string BackColor { get; set; }
    public string TextColor { get; set; }
}
