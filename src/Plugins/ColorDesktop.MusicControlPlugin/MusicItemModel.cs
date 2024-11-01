using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using ColorDesktop.MusicControlPlugin.Hook;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.MusicControlPlugin;

public partial class MusicItemModel : ObservableObject
{
    [ObservableProperty]
    private string _title;
    [ObservableProperty]
    private string _subTitle;

    [ObservableProperty]
    private Bitmap _image;
}
