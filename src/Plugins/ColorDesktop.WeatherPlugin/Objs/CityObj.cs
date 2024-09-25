using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorDesktop.WeatherPlugin.Objs;

public record CityObj
{
    public string ID { get; set; }
    public string Name { get; set; }
    public string NameD { get; set; }
    public string NameE { get; set; }
    public List<CityObj> Childs { get; set; }
}
