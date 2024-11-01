using Windows.Media.Control;

namespace ColorDesktop.MusicControlPlugin.Hook;

#pragma warning disable CA1416 // 验证平台兼容性

public class Win32Hook : IHook
{
    private readonly GlobalSystemMediaTransportControlsSessionManager _manager;

    public event Action<int>? SessionAdd;
    public event Action<int>? SessionRemove;
    public event Action<int, MediaProperties>? MusicChange;
    public event Action<int, PlaybackInfo>? StateChange;
    public event Action<int, Timeline>? TimeLineChange;

    private readonly Dictionary<int, GlobalSystemMediaTransportControlsSession> _list = [];

    public Win32Hook(GlobalSystemMediaTransportControlsSessionManager manager)
    {
        _manager = manager;

        foreach (var item in _manager.GetSessions())
        {
            OpenSession(item);
        }

        _manager.SessionsChanged += Manager_SessionsChanged;
    }

    private void Manager_SessionsChanged(GlobalSystemMediaTransportControlsSessionManager sender, SessionsChangedEventArgs args)
    {
        var list = sender.GetSessions();

        foreach (var item in _list.ToArray())
        {
            if (list.Contains(item.Value))
            {
                continue;
            }

            CloseSession(item.Value);
            SessionRemove?.Invoke(item.Key);
        }

        foreach (var item in list)
        {
            if (_list.ContainsValue(item))
            {
                continue;
            }

            OpenSession(item);
            SessionAdd?.Invoke(item.GetHashCode());
        }
    }

    private void CloseSession(GlobalSystemMediaTransportControlsSession session)
    {
        session.MediaPropertiesChanged -= Session_MediaPropertiesChanged;
        session.PlaybackInfoChanged -= Session_PlaybackInfoChanged;
        session.TimelinePropertiesChanged -= Session_TimelinePropertiesChanged;
        _list.Remove(session.GetHashCode());
    }

    private void OpenSession(GlobalSystemMediaTransportControlsSession session)
    {
        session.MediaPropertiesChanged += Session_MediaPropertiesChanged;
        session.PlaybackInfoChanged += Session_PlaybackInfoChanged;
        session.TimelinePropertiesChanged += Session_TimelinePropertiesChanged;

        _list.Add(session.GetHashCode(), session);
    }

    private void Session_TimelinePropertiesChanged(GlobalSystemMediaTransportControlsSession sender,
        TimelinePropertiesChangedEventArgs args)
    {
        var info = sender.GetTimelineProperties();
        TimeLineChange?.Invoke(sender.GetHashCode(), info.Build());
    }

    private void Session_PlaybackInfoChanged(GlobalSystemMediaTransportControlsSession sender,
        PlaybackInfoChangedEventArgs args)
    {
        var info = sender.GetPlaybackInfo();
        StateChange?.Invoke(sender.GetHashCode(), info.Build());
    }

    private async void Session_MediaPropertiesChanged(GlobalSystemMediaTransportControlsSession sender,
        MediaPropertiesChangedEventArgs args)
    {
        var info = await sender.TryGetMediaPropertiesAsync();
        if (info == null)
        {
            return;
        }
        MusicChange?.Invoke(sender.GetHashCode(), await info.Build());
    }

    public void Stop()
    {
        _manager.SessionsChanged -= Manager_SessionsChanged;
        foreach (var item in _list)
        {
            CloseSession(item.Value);
        }
        _list.Clear();
    }

    public async Task<MediaProperties?> GetProperties(int item)
    {
        if (_list.TryGetValue(item, out var session))
        {
            try
            {
                var info = await session.TryGetMediaPropertiesAsync();
                if (info != null)
                {
                    return await info.Build();
                }
            }
            catch(Exception e)
            { 

            }
        }
        return null;
    }

    public PlaybackInfo? GetPlaybackInfo(int item)
    {
        if (_list.TryGetValue(item, out var session))
        {
            return session.GetPlaybackInfo().Build();
        }
        return null;
    }

    public Timeline? GetTimeline(int item)
    {
        if (_list.TryGetValue(item, out var session))
        {
            return session.GetTimelineProperties().Build();
        }
        return null;
    }

    public async void Pause(int id)
    {
        if (_list.TryGetValue(id, out var session)
            && session.GetPlaybackInfo().Controls.IsPauseEnabled)
        {
            await session.TryPauseAsync();
        }
    }

    public async void Play(int id)
    {
        if (_list.TryGetValue(id, out var session)
            && session.GetPlaybackInfo().Controls.IsPlayEnabled)
        {
            await session.TryPlayAsync();
        }
    }

    public async void Next(int id)
    {
        if (_list.TryGetValue(id, out var session)
            && session.GetPlaybackInfo().Controls.IsNextEnabled)
        {
            await session.TrySkipNextAsync();
        }
    }

    public async void Last(int id)
    {
        if (_list.TryGetValue(id, out var session)
            && session.GetPlaybackInfo().Controls.IsPreviousEnabled)
        {
            await session.TrySkipPreviousAsync();
        }
    }

    public IEnumerable<int> GetList()
    {
        return _list.Keys;
    }

    public string? GetName(int item)
    {
        if (_list.TryGetValue(item, out var session))
        {
            return session.SourceAppUserModelId;
        }
        return null;
    }
}

public static class Win32
{
    public static async Task<Win32Hook?> Init()
    {
        var res = await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
        if (res != null)
        {
            return new Win32Hook(res);
        }

        return null;
    }

    public static PlaybackInfo Build(this GlobalSystemMediaTransportControlsSessionPlaybackInfo info)
    {
        return new()
        {
            AutoRepeatMode = (AutoRepeatMode?)info.AutoRepeatMode,
            Controls = new()
            {
                IsPauseEnabled = info.Controls.IsPauseEnabled,
                IsRewindEnabled = info.Controls.IsRewindEnabled,
                IsRepeatEnabled = info.Controls.IsRepeatEnabled,
                IsRecordEnabled = info.Controls.IsRecordEnabled,
                IsPreviousEnabled = info.Controls.IsPreviousEnabled,
                IsPlaybackRateEnabled = info.Controls.IsPlaybackRateEnabled,
                IsPlaybackPositionEnabled = info.Controls.IsPlaybackPositionEnabled,
                IsPlayPauseToggleEnabled = info.Controls.IsPlayPauseToggleEnabled,
                IsPlayEnabled = info.Controls.IsPlayEnabled,
                IsShuffleEnabled = info.Controls.IsShuffleEnabled,
                IsStopEnabled = info.Controls.IsStopEnabled,
                IsFastForwardEnabled = info.Controls.IsFastForwardEnabled,
                IsChannelUpEnabled = info.Controls.IsChannelUpEnabled,
                IsChannelDownEnabled = info.Controls.IsChannelDownEnabled,
                IsNextEnabled = info.Controls.IsNextEnabled
            },
            IsShuffleActive = info.IsShuffleActive,
            PlaybackRate = info.PlaybackRate,
            PlaybackStatus = (PlaybackStatus)info.PlaybackStatus,
            PlaybackType = (PlaybackType?)info.PlaybackType
        };
    }

    public static async Task<MediaProperties> Build(this GlobalSystemMediaTransportControlsSessionMediaProperties info)
    {
        var mediaProperties = new MediaProperties
        {
            AlbumArtist = info.AlbumArtist,
            AlbumTitle = info.AlbumTitle,
            AlbumTrackCount = info.AlbumTrackCount,
            Artist = info.Artist,
            Genres = new(info.Genres),
            PlaybackType = (PlaybackType?)info.PlaybackType,
            Subtitle = info.Subtitle,
            Title = info.Title,
            TrackNumber = info.TrackNumber
        };
        if (info.Thumbnail != null)
        {
            using var mem = new MemoryStream();
            var data = await info.Thumbnail.OpenReadAsync();
            data.AsStreamForRead().CopyTo(mem);
            mediaProperties.Thumbnail = mem.ToArray();
        }

        return mediaProperties;
    }

    public static Timeline Build(this GlobalSystemMediaTransportControlsSessionTimelineProperties info)
    {
        return new Timeline
        {
            StartTime = info.StartTime,
            Position = info.Position,
            EndTime = info.EndTime,
            LastUpdatedTime = info.LastUpdatedTime,
            MaxSeekTime = info.MaxSeekTime,
            MinSeekTime = info.MinSeekTime
        };
    }
}

#pragma warning restore CA1416 // 验证平台兼容性
