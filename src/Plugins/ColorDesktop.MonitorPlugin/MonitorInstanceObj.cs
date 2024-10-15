using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorDesktop.Api;

namespace ColorDesktop.MonitorPlugin;

public enum MonitorDisplayType
{ 
    Text, Number
}

public enum PanelType
{
    Stack, Wrap, Panel
}

public record MonitorItemObj
{
    public MonitorDisplayType Display { get; set; }
    public string Name { get; set; }
    public MarginObj Margin { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public string Sensor { get; set; }
}

public record MonitorInstanceObj
{
    public int Width { get; set; }
    public int Height { get; set; }
    public bool AutoSize { get; set; }
    public PanelType PanelType { get; set; }
    public List<MonitorItemObj> Items { get; set; }
}
