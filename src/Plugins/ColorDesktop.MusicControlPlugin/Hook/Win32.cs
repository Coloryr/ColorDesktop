using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Control;

namespace ColorDesktop.MusicControlPlugin.Hook;

#pragma warning disable CA1416 // 验证平台兼容性

public class Win32Hook : IHook
{
    private readonly GlobalSystemMediaTransportControlsSessionManager _manager;
    private GlobalSystemMediaTransportControlsSession? _session;

    public event Action? SessionChange;
    public event Action<MediaProperties>? MusicChange;
    public event Action<PlaybackInfo>? StateChange;
    public event Action<Timeline>? TimeLineChange;

    private readonly Timeline _timeline = new();
    private readonly PlaybackInfo _playbackInfo = new()
    { 
        Controls = new()
    };
    private readonly MediaProperties _mediaProperties = new()
    {
        Genres = []
    };

    public Win32Hook(GlobalSystemMediaTransportControlsSessionManager manager)
    {
        _manager = manager;
        _manager.CurrentSessionChanged += Res_CurrentSessionChanged;

        SessionChanged();
    }

    private void Res_CurrentSessionChanged(GlobalSystemMediaTransportControlsSessionManager sender, CurrentSessionChangedEventArgs args)
    {
        SessionChanged();
    }

    private void SessionChanged()
    {
        var session = _manager.GetCurrentSession();
        CloseSession();
        if (session != null)
        {
            _session = session;
            OpenSession();
        }
    }

    private void CloseSession()
    {
        if (_session == null)
        {
            return;
        }

        _session.MediaPropertiesChanged -= Session_MediaPropertiesChanged;
        _session.PlaybackInfoChanged -= Session_PlaybackInfoChanged;
        _session.TimelinePropertiesChanged -= Session_TimelinePropertiesChanged;

        _session = null;
    }

    private void OpenSession()
    {
        if (_session == null)
        {
            return;
        }

        _session.MediaPropertiesChanged += Session_MediaPropertiesChanged;
        _session.PlaybackInfoChanged += Session_PlaybackInfoChanged;
        _session.TimelinePropertiesChanged += Session_TimelinePropertiesChanged;
    }

    private void Session_TimelinePropertiesChanged(GlobalSystemMediaTransportControlsSession sender,
        TimelinePropertiesChangedEventArgs args)
    {
        var timeline = sender.GetTimelineProperties();
        _timeline.StartTime = timeline.StartTime;
        _timeline.Position = timeline.Position;
        _timeline.EndTime = timeline.EndTime;
        _timeline.LastUpdatedTime = timeline.LastUpdatedTime;
        _timeline.MaxSeekTime = timeline.MaxSeekTime;
        _timeline.MinSeekTime = timeline.MinSeekTime;
        TimeLineChange?.Invoke(_timeline);
    }

    private void Session_PlaybackInfoChanged(GlobalSystemMediaTransportControlsSession sender,
        PlaybackInfoChangedEventArgs args)
    {
        var info = sender.GetPlaybackInfo();

        _playbackInfo.AutoRepeatMode = (AutoRepeatMode?)info.AutoRepeatMode;
        _playbackInfo.Controls.IsPauseEnabled = info.Controls.IsPauseEnabled;
        _playbackInfo.Controls.IsRewindEnabled = info.Controls.IsRewindEnabled;
        _playbackInfo.Controls.IsRepeatEnabled = info.Controls.IsRepeatEnabled;
        _playbackInfo.Controls.IsRecordEnabled = info.Controls.IsRecordEnabled;
        _playbackInfo.Controls.IsPreviousEnabled = info.Controls.IsPreviousEnabled;
        _playbackInfo.Controls.IsPlaybackRateEnabled = info.Controls.IsPlaybackRateEnabled;
        _playbackInfo.Controls.IsPlaybackPositionEnabled = info.Controls.IsPlaybackPositionEnabled;
        _playbackInfo.Controls.IsPlayPauseToggleEnabled = info.Controls.IsPlayPauseToggleEnabled;
        _playbackInfo.Controls.IsPlayEnabled = info.Controls.IsPlayEnabled;
        _playbackInfo.Controls.IsShuffleEnabled = info.Controls.IsShuffleEnabled;
        _playbackInfo.Controls.IsStopEnabled = info.Controls.IsStopEnabled;
        _playbackInfo.Controls.IsFastForwardEnabled = info.Controls.IsFastForwardEnabled;
        _playbackInfo.Controls.IsChannelUpEnabled = info.Controls.IsChannelUpEnabled;
        _playbackInfo.Controls.IsChannelDownEnabled = info.Controls.IsChannelDownEnabled;
        _playbackInfo.Controls.IsNextEnabled = info.Controls.IsNextEnabled;
        _playbackInfo.IsShuffleActive = info.IsShuffleActive;
        _playbackInfo.PlaybackRate = info.PlaybackRate;
        _playbackInfo.PlaybackStatus = (PlaybackStatus)info.PlaybackStatus;
        _playbackInfo.PlaybackType = (PlaybackType?)info.PlaybackType;

        StateChange?.Invoke(_playbackInfo);
    }

    private async void Session_MediaPropertiesChanged(GlobalSystemMediaTransportControlsSession sender,
        MediaPropertiesChangedEventArgs args)
    {
        var info = await sender.TryGetMediaPropertiesAsync();

        _mediaProperties.AlbumArtist = info.AlbumArtist;
        _mediaProperties.AlbumTitle = info.AlbumTitle;
        _mediaProperties.AlbumTrackCount = info.AlbumTrackCount;
        _mediaProperties.Artist = info.Artist;
        _mediaProperties.Genres = new(info.Genres);
        _mediaProperties.PlaybackType = (PlaybackType?)info.PlaybackType;
        _mediaProperties.Subtitle = info.Subtitle;
        if (info.Thumbnail != null)
        {
            using var mem = new MemoryStream();
            var data = await info.Thumbnail.OpenReadAsync();
            data.AsStreamForRead().CopyTo(mem);
            _mediaProperties.Thumbnail = mem.ToArray();
        }
        else
        {
            _mediaProperties.Thumbnail = null;
        }
        _mediaProperties.Title = info.Title;
        _mediaProperties.TrackNumber = info.TrackNumber;

        MusicChange?.Invoke(_mediaProperties);
    }

    public void Stop()
    {
        _manager.CurrentSessionChanged -= Res_CurrentSessionChanged;
        CloseSession();
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
}

#pragma warning restore CA1416 // 验证平台兼容性
