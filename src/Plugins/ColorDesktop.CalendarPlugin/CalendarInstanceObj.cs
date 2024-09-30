using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorDesktop.CalendarPlugin;

public enum WeekStart
{
    DaySun,
    DaySat,
    DayOne
}

public record CalendarInstanceObj
{
    public string BackColor { get; set; }
    public string TextColor { get; set; }
    public WeekStart WeekStart { get; set; }
}
