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


public record City1Obj
{
    public int Adcode { get; set; }
    public int Citycode { get; set; }
    public string Name { get; set; }
    public List<City1Obj> Childs { get; set; }
}
