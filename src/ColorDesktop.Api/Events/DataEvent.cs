using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorDesktop.Api.Events;

public class DataEvent : BaseEvent
{
    public object?[] Objects { get; set; }
}
