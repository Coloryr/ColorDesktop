using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExCSS;
using Tmds.DBus.Protocol;

namespace ColorDesktop.MusicControlPlugin.Hook;

public class PlayerItem
{
    private readonly MprisService _service;
    private readonly Player _player;
    private readonly LinuxHook _top;
    private readonly int _id;

    private IDisposable _hook;

    public PlayerItem(Connection connection, string player, LinuxHook top)
    {
        _id = player.GetHashCode();
        _top = top;
        _service = new MprisService(connection, player);
        _player = _service.CreatePlayer("/org/mpris/MediaPlayer2");
    }

    public async Task Hook()
    {
        _hook = await _player.WatchPropertiesChangedAsync(OnPropertiesChanged);
    }

    private async void OnPropertiesChanged(Exception? ex, PropertyChanges<PlayerProperties> changes)
    {
        if (ex is not null)
        {
            return;
        }

        try
        {
            if (changes.IsInvalidated(nameof(PlayerProperties.Metadata))
            || changes.HasChanged(nameof(PlayerProperties.Metadata)))
            {
                _top.MetadataChange(_id, await GetProperties());
                _top.TimelineChange(_id, await GetTimeline());
            }
            else if (changes.IsInvalidated(nameof(PlayerProperties.PlaybackStatus))
                || changes.IsInvalidated(nameof(PlayerProperties.LoopStatus))
                || changes.IsInvalidated(nameof(PlayerProperties.Shuffle))
                || changes.IsInvalidated(nameof(PlayerProperties.Rate))
                || changes.IsInvalidated(nameof(PlayerProperties.CanGoNext))
                || changes.IsInvalidated(nameof(PlayerProperties.CanGoPrevious))
                || changes.IsInvalidated(nameof(PlayerProperties.CanPlay))
                || changes.IsInvalidated(nameof(PlayerProperties.CanPause))
                || changes.HasChanged(nameof(PlayerProperties.PlaybackStatus))
                || changes.HasChanged(nameof(PlayerProperties.LoopStatus))
                || changes.HasChanged(nameof(PlayerProperties.Shuffle))
                || changes.HasChanged(nameof(PlayerProperties.Rate))
                || changes.HasChanged(nameof(PlayerProperties.CanGoNext))
                || changes.HasChanged(nameof(PlayerProperties.CanGoPrevious))
                || changes.HasChanged(nameof(PlayerProperties.CanPlay))
                || changes.HasChanged(nameof(PlayerProperties.CanPause)))
            {
                _top.PlaybackInfoChange(_id, await GetPlaybackInfo());
            }
            else if (changes.IsInvalidated(nameof(PlayerProperties.Position))
                || changes.HasChanged(nameof(PlayerProperties.Position)))
            {
                _top.TimelineChange(_id, await GetTimeline());
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error while handling player properties changed: {e}");
        }
    }

    public Task PreviousAsync() => _player.PreviousAsync();

    public Task NextAsync() => _player.NextAsync();

    public Task PlayPauseAsync() => _player.PlayPauseAsync();

    public string? GetName()
    {
        return _service.Destination;
    }

    public async Task<PlaybackInfo> GetPlaybackInfo()
    {
        var info = new PlaybackInfo()
        {
            Controls = new()
        };
        var loop = await _player.GetLoopStatusAsync();
        if (loop == "None")
        {
            info.AutoRepeatMode = AutoRepeatMode.None;
        }
        else if (loop == "Track")
        {
            info.AutoRepeatMode = AutoRepeatMode.Track;
        }
        else if (loop == "Playlist")
        {
            info.AutoRepeatMode = AutoRepeatMode.List;
        }
        var rate = await _player.GetRateAsync();
        info.PlaybackRate = rate;
        var state = await _player.GetPlaybackStatusAsync();
        if (state == "Playing")
        {
            info.PlaybackStatus = PlaybackStatus.Playing;
        }
        else if (state == "Paused")
        {
            info.PlaybackStatus = PlaybackStatus.Paused;
        }
        else if (state == "Stopped")
        {
            info.PlaybackStatus = PlaybackStatus.Stopped;
        }
        var shuffle = await _player.GetShuffleAsync();
        info.IsShuffleActive = shuffle;
        info.Controls.IsPauseEnabled = await _player.GetCanPauseAsync();
        info.Controls.IsPlayEnabled = await _player.GetCanPlayAsync();
        info.Controls.IsNextEnabled = await _player.GetCanGoNextAsync();
        info.Controls.IsPreviousEnabled = await _player.GetCanGoPreviousAsync();

        return info;
    }

    public async Task<MediaProperties> GetProperties()
    {
        var info = new MediaProperties();
        var metadata = await _player.GetMetadataAsync();
        if (metadata.TryGetValue("xesam:title", out var value))
        {
            info.Title = value.GetString();
        }
        if (metadata.TryGetValue("xesam:artist", out value))
        {
            var list = value.GetArray<string>();
            var data = Build(list);
            info.Artist = data;
            info.Subtitle = data;
            info.AlbumArtist = data;
        }
        if (metadata.TryGetValue("xesam:album", out value))
        {
            info.AlbumTitle = value.GetString();
        }
        if (metadata.TryGetValue("xesam:genre", out value))
        {
            var list = value.GetArray<string>();
            info.Genres = new(list);
        }
        if (metadata.TryGetValue("xesam:trackNumber", out value))
        {
            info.TrackNumber = value.GetInt32();
        }
        if (metadata.TryGetValue("mpris:artUrl", out value))
        {
            var file = value.GetString();
            var uri = new Uri(file);
            if (uri.IsFile)
            {
                string filePath = uri.LocalPath;
                try
                {
                    info.Thumbnail = File.ReadAllBytes(filePath);
                }
                catch
                {

                }
            }
        }

        return info;
    }

    public async Task<Timeline> GetTimeline()
    {
        var time = new Timeline();
        var data1 = await _player.GetMetadataAsync();
        var data2 = await _player.GetPositionAsync();

        if (data1.TryGetValue("mpris:length", out var value))
        {
            time.EndTime = TimeSpan.FromMicroseconds(value.GetInt64());
        }
        time.Position = TimeSpan.FromMicroseconds(data2);

        return time;
    }

    public void Close()
    {
        _hook?.Dispose();
    }

    private static string Build(string[] list)
    {
        if (list.Length == 0)
        {
            return "";
        }
        var builder = new StringBuilder();
        foreach (var item in list)
        {
            builder.Append(item).Append(',');
        }
        return builder.ToString()[..^1];
    }
}

public class LinuxHook : IHook
{
    public const string MediaPlayerService = "org.mpris.MediaPlayer2.";

    public event Action<int>? SessionAdd;
    public event Action<int>? SessionRemove;
    public event Action<int, MediaProperties>? MusicChange;
    public event Action<int, PlaybackInfo>? StateChange;
    public event Action<int, Timeline>? TimeLineChange;

    private readonly Dictionary<string, PlayerItem> _players = [];
    private readonly Dictionary<int, PlayerItem> _playerHash = [];

    private readonly Connection _connection;

    private bool _isRun;

    public LinuxHook(Connection connection)
    {
        _connection = connection;

        ScanPlayer().ContinueWith((a) =>
        {
            _isRun = true;
            new Thread(() =>
            {
                while (_isRun)
                {
                    Thread.Sleep(500);
                    try
                    {
                        ScanPlayer().Wait();
                    }
                    catch
                    {

                    }
                }
            }).Start();
        });
    }

    private async Task ScanPlayer()
    {
        var services = await _connection.ListServicesAsync();

        var items = services.Where(service => service.StartsWith(MediaPlayerService, StringComparison.Ordinal));

        foreach (var item in items)
        {
            if (_players.ContainsKey(item))
            {
                continue;
            }
            try
            {
                var player = new PlayerItem(_connection, item, this);
                await player.Hook();
                _players.Add(item, player);
                _playerHash.Add(item.GetHashCode(), player);
                SessionAdd?.Invoke(item.GetHashCode());
            }
            catch
            {

            }
        }

        foreach (var item in _players.ToArray())
        {
            if (items.Contains(item.Key))
            {
                continue;
            }

            item.Value.Close();
            _players.Remove(item.Key);
            _playerHash.Remove(item.Key.GetHashCode());
            SessionRemove?.Invoke(item.Key.GetHashCode());
        }
    }

    public IEnumerable<int> GetList()
    {
        var list = new List<int>();
        foreach (var item in _players)
        {
            list.Add(item.Key.GetHashCode());
        }
        return list;
    }

    public string? GetName(int item)
    {
        if (_playerHash.TryGetValue(item, out var player))
        {
            return player.GetName();
        }

        return null;
    }

    public async Task<PlaybackInfo?> GetPlaybackInfo(int item)
    {
        if (_playerHash.TryGetValue(item, out var player))
        {
            return await player.GetPlaybackInfo();
        }

        return null;
    }

    public async Task<MediaProperties?> GetProperties(int item)
    {
        if (_playerHash.TryGetValue(item, out var player))
        {
            return await player.GetProperties();
        }

        return null;
    }

    public async Task<Timeline?> GetTimeline(int item)
    {
        if (_playerHash.TryGetValue(item, out var player))
        {
            return await player.GetTimeline();
        }

        return null;
    }

    public async void Last(int id)
    {
        if (_playerHash.TryGetValue(id, out var player))
        {
            await player.PreviousAsync();
        }
    }

    public async void Next(int id)
    {
        if (_playerHash.TryGetValue(id, out var player))
        {
            await player.NextAsync();
        }
    }

    public async void Pause(int id)
    {
        if (_playerHash.TryGetValue(id, out var player))
        {
            await player.PlayPauseAsync();
        }
    }

    public async void Play(int id)
    {
        if (_playerHash.TryGetValue(id, out var player))
        {
            await player.PlayPauseAsync();
        }
    }

    public void Stop()
    {
        _isRun = false;

        foreach (var item in _players)
        {
            item.Value.Close();
        }

        _players.Clear();
    }

    public void MetadataChange(int id, MediaProperties info)
    {
        MusicChange?.Invoke(id, info);
    }

    public void TimelineChange(int id, Timeline timeline)
    {
        TimeLineChange?.Invoke(id, timeline);
    }

    public void PlaybackInfoChange(int id, PlaybackInfo playbackInfo)
    {
        StateChange?.Invoke(id, playbackInfo);
    }
}

public static class Linux
{
    public static async Task<LinuxHook?> Init()
    {
        try
        {
            // Connect to the session bus.
            var connection = new Connection(Address.Session!);
            await connection.ConnectAsync();

            return new LinuxHook(connection);
        }
        catch (Exception e)
        {

        }
        return null;
    }
}