using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using ColorDesktop.MusicControlPlugin.Hook;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.MusicControlPlugin;

public partial class MusicModel : ObservableObject
{
    public ObservableCollection<MusicItemModel> Items { get; init; } = [];

    [ObservableProperty]
    private bool _isEmpty;

    private readonly Dictionary<int, MusicItemModel> _items = [];

    public void Init()
    {
        if (MusicControlPlugin.MusicHook != null)
        {
            MusicControlPlugin.MusicHook.SessionAdd += MusicHook_SessionAdd;
            MusicControlPlugin.MusicHook.SessionRemove += MusicHook_SessionRemove;
            MusicControlPlugin.MusicHook!.StateChange += MusicHook_StateChange;
            MusicControlPlugin.MusicHook!.MusicChange += MusicItemModel_MusicChange;

            foreach (var item in MusicControlPlugin.MusicHook.GetList())
            {
                MusicHook_SessionAdd(item);
            }

            IsEmpty = Items.Count == 0;
        }
    }

    private void MusicHook_SessionRemove(int obj)
    {
        if (_items.Remove(obj, out var item))
        {
            Items.Remove(item);
        }

        IsEmpty = Items.Count == 0;
    }

    private async void MusicHook_SessionAdd(int obj)
    {
        var item = new MusicItemModel(obj);
        Items.Add(item);
        _items.Add(obj, item);

        item.Player = MusicControlPlugin.MusicHook!.GetName(obj) ?? "";

        var data = await MusicControlPlugin.MusicHook!.GetProperties(obj);
        if (data != null )
        {
            item.Title = data.Title;
            item.SubTitle = data.Artist;
            if (data.Thumbnail != null)
            {
                using var stream = new MemoryStream(data.Thumbnail);
                item.Image = Bitmap.DecodeToWidth(stream, 200);
            }
            else
            {
                var bitmap = item.Image;
                item.Image = null;
                bitmap?.Dispose();
            }
        }

        var data1 = await MusicControlPlugin.MusicHook.GetPlaybackInfo(obj);
        if (data1 != null)
        {
            item.IsPlay = data1.PlaybackStatus == PlaybackStatus.Playing;
        }

        IsEmpty = Items.Count == 0;
    }


    private void MusicItemModel_MusicChange(int arg1, MediaProperties arg2)
    {
        if (_items.TryGetValue(arg1, out var item))
        {
            item.Title = arg2.Title;
            item.SubTitle = arg2.Artist;

            if (arg2.Thumbnail != null)
            {
                using var stream = new MemoryStream(arg2.Thumbnail);
                item.Image = new Bitmap(stream);
            }
            else
            {
                var bitmap = item.Image;
                item.Image = null;
                bitmap?.Dispose();
            }
        }
    }

    private void MusicHook_StateChange(int arg1, PlaybackInfo arg2)
    {
        if (_items.TryGetValue(arg1, out var item))
        {
            item.IsPlay = arg2.PlaybackStatus == PlaybackStatus.Playing;
        }
    }

    public void Stop()
    {
        if (MusicControlPlugin.MusicHook != null)
        {
            MusicControlPlugin.MusicHook.SessionAdd -= MusicHook_SessionAdd;
            MusicControlPlugin.MusicHook.SessionRemove -= MusicHook_SessionRemove;
            MusicControlPlugin.MusicHook!.StateChange -= MusicHook_StateChange;
            MusicControlPlugin.MusicHook!.MusicChange -= MusicItemModel_MusicChange;
        }
    }
}
