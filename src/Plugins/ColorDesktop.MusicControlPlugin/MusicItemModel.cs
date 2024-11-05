using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ColorDesktop.MusicControlPlugin;

public partial class MusicItemModel(int id) : ObservableObject
{
    private static readonly string[] PlayIcons =
    [
        "/Resource/icon1.svg",
        "/Resource/icon2.svg"
    ];

    [ObservableProperty]
    private string _title;
    [ObservableProperty]
    private string _subTitle;
    [ObservableProperty]
    private string _player;

    [ObservableProperty]
    private Bitmap? _image;

    [ObservableProperty]
    private string _playIcon = PlayIcons[0];

    private bool _isplay;

    public bool IsPlay
    {
        set
        {
            PlayIcon = PlayIcons[value ? 1 : 0];
            _isplay = value;
        }
    }

    [RelayCommand]
    public void Play()
    {
        if (_isplay)
        {
            MusicControlPlugin.MusicHook!.Pause(id);
        }
        else
        {
            MusicControlPlugin.MusicHook!.Play(id);
        }
    }

    [RelayCommand]
    public void Last()
    {
        MusicControlPlugin.MusicHook!.Last(id);
    }

    [RelayCommand]
    public void Next()
    {
        MusicControlPlugin.MusicHook!.Next(id);
    }
}
