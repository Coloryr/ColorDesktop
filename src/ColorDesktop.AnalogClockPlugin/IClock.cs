using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorDesktop.AnalogClockPlugin;

public interface IClock
{
    void Update(AnalogClockConfigObj obj);
    void Tick();
}
