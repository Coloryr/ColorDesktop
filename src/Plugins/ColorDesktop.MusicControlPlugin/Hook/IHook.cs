using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorDesktop.MusicControlPlugin.Hook;

public interface IHook
{
    event Action<int>? SessionAdd;
    event Action<int>? SessionRemove;
    event Action<int, MediaProperties>? MusicChange;
    event Action<int, PlaybackInfo>? StateChange;
    event Action<int, Timeline>? TimeLineChange;

    Task<MediaProperties?> GetProperties(int item);
    PlaybackInfo? GetPlaybackInfo(int item);
    Timeline? GetTimeline(int item);

    void Stop();
}
