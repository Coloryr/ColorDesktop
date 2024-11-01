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

    private readonly Dictionary<int, MusicItemModel> _items = [];

    public void Init()
    {
        if (MusicControlPlugin.MusicHook != null)
        {
            MusicControlPlugin.MusicHook.SessionAdd += MusicHook_SessionAdd;
            MusicControlPlugin.MusicHook.SessionRemove += MusicHook_SessionRemove;
            MusicControlPlugin.MusicHook!.StateChange += MusicHook_StateChange;
            MusicControlPlugin.MusicHook!.MusicChange += MusicItemModel_MusicChange;
        }
    }

    private void MusicHook_SessionRemove(int obj)
    {
        if (_items.Remove(obj, out var item))
        {
            Items.Remove(item);
        }
    }

    private async void MusicHook_SessionAdd(int obj)
    {
        var item = new MusicItemModel();
        Items.Add(item);
        _items.Add(obj, item);

        var data = await MusicControlPlugin.MusicHook!.GetProperties(obj);
        if (data == null)
        {
            return;
        }
        item.Title = data.Title;
    }

    private void MusicItemModel_MusicChange(int arg1, MediaProperties arg2)
    {

    }

    private void MusicHook_StateChange(int arg1, PlaybackInfo arg2)
    {

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
