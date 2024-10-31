using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorDesktop.MusicControlPlugin.Hook;

public interface IHook
{
    event Action? SessionChange;
    event Action<MediaProperties>? MusicChange;
    event Action<PlaybackInfo>? StateChange;
    event Action<Timeline>? TimeLineChange;

    void Stop();
}
