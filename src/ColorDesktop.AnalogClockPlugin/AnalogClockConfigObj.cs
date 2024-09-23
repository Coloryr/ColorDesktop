using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorDesktop.AnalogClockPlugin;

public enum ClockType
{
    Analog, Digital
}

public record AnalogClockConfigObj
{
    public ClockType Type { get; set; }
    public int Size { get; set; }
    public bool DisplaySecond { get; set; }
    public string Color { get; set; }
    public bool Blink { get; set; }
}
