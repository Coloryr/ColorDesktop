using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Threading;
using ColorDesktop.MusicControlPlugin.Hook;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.MusicControlPlugin;

public partial class MusicModel : ObservableObject
{
    [ObservableProperty]
    private string _name;

    public void Init()
    {
        if (MusicControlPlugin.MusicHook != null)
        {
            MusicControlPlugin.MusicHook.MusicChange += MusicHook_MusicChange;
        }
    }

    public void Stop()
    {
        if (MusicControlPlugin.MusicHook != null)
        {
            MusicControlPlugin.MusicHook.MusicChange -= MusicHook_MusicChange;
        }
    }

    private void MusicHook_MusicChange(MediaProperties obj)
    {
        Dispatcher.UIThread.Post(() =>
        {
            Name = obj.Title;
        });
    }
}
