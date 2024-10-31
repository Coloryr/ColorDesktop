using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorDesktop.MusicControlPlugin.Hook;

public class Timeline
{
    public TimeSpan EndTime;
    public DateTimeOffset LastUpdatedTime;
    public TimeSpan MaxSeekTime;
    public TimeSpan MinSeekTime;
    public TimeSpan Position;
    public TimeSpan StartTime;
}
