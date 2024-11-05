namespace ColorDesktop.MusicControlPlugin.Hook;

public interface IHook
{
    event Action<int>? SessionAdd;
    event Action<int>? SessionRemove;
    event Action<int, MediaProperties>? MusicChange;
    event Action<int, PlaybackInfo>? StateChange;
    event Action<int, Timeline>? TimeLineChange;

    string? GetName(int item);
    Task<MediaProperties?> GetProperties(int item);
    Task<PlaybackInfo?> GetPlaybackInfo(int item);
    Task<Timeline?> GetTimeline(int item);

    void Pause(int id);
    void Play(int id);
    void Next(int id);
    void Last(int id);

    void Stop();
    IEnumerable<int> GetList();
}
