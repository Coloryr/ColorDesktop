namespace ColorDesktop.MusicControlPlugin.Hook;

public enum AutoRepeatMode
{
    None = 0,
    Track = 1,
    List = 2
}

public enum PlaybackStatus
{
    Closed = 0,
    Opened = 1,
    Changing = 2,
    Stopped = 3,
    Playing = 4,
    Paused = 5
}

public enum PlaybackType
{
    Unknown = 0,
    Music = 1,
    Video = 2,
    Image = 3
}

public record PlaybackControls
{
    public bool IsPauseEnabled;
    public bool IsRewindEnabled;
    public bool IsRepeatEnabled;
    public bool IsRecordEnabled;
    public bool IsPreviousEnabled;
    public bool IsPlaybackRateEnabled;
    public bool IsPlaybackPositionEnabled;
    public bool IsPlayPauseToggleEnabled;
    public bool IsPlayEnabled;
    public bool IsShuffleEnabled;
    public bool IsStopEnabled;
    public bool IsFastForwardEnabled;
    public bool IsChannelUpEnabled;
    public bool IsChannelDownEnabled;
    public bool IsNextEnabled;
}

public class PlaybackInfo
{
    public AutoRepeatMode? AutoRepeatMode;
    public PlaybackControls Controls;
    public bool? IsShuffleActive;
    public double? PlaybackRate;
    public PlaybackStatus PlaybackStatus;
    public PlaybackType? PlaybackType;
}
