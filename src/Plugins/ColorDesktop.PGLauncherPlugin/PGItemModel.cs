﻿using System.Diagnostics;
using System.Reflection;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.PGLauncherPlugin;

public partial class PGItemModel : ObservableObject
{
    public string Name => _obj.Name;
    public int Size => _obj.Size;
    public int TextSize => _obj.TextSize;
    public Thickness Thickness => new(_obj.Margin.Left, _obj.Margin.Top, _obj.Margin.Right, _obj.Margin.Bottom);
    public Thickness Border => new(_obj.BorderSize);
    public IBrush BackColor => Brush.Parse(_obj.BackColor);
    public IBrush TextColor => Brush.Parse(_obj.TextColor);

    public bool DisplayIcon { get; init; }
    public bool DisplayText { get; init; }
    public Bitmap Icon { get; init; }

    [ObservableProperty]
    private bool _isOver;

    private readonly PGItemObj _obj;

    public PGItemModel(PGItemObj obj)
    {
        _obj = obj;

        switch (obj.Display)
        {
            case DisplayType.Text:
                DisplayIcon = false;
                DisplayText = true;
                break;
            case DisplayType.Img:
            case DisplayType.Icon:
                DisplayIcon = true;
                DisplayText = false;
                break;
            default:
                DisplayIcon = true;
                DisplayText = true;
                break;
        }

        var assm = Assembly.GetExecutingAssembly();

        if (obj.Display is DisplayType.Img or DisplayType.TextImg)
        {
            if (!string.IsNullOrWhiteSpace(obj.Icon) && File.Exists(obj.Icon))
            {
                try
                {
                    using var stream = File.OpenRead(obj.Icon);
                    Icon = Bitmap.DecodeToWidth(stream, obj.Size);
                }
                catch
                {
                    using var item = assm.GetManifestResourceStream("ColorDesktop.PGLauncherPlugin.Resource.image2.png")!;
                    Icon = new Bitmap(item);
                }
            }
            else
            {
                using var item = assm.GetManifestResourceStream("ColorDesktop.PGLauncherPlugin.Resource.image1.png")!;
                Icon = new Bitmap(item);
            }
        }
        else if (obj.Display is DisplayType.Icon or DisplayType.TextIcon)
        {
            if (!SystemUtils.IsExecutable(obj.Local))
            {
                using var item = assm.GetManifestResourceStream("ColorDesktop.PGLauncherPlugin.Resource.image1.png")!;
                Icon = new Bitmap(item);
            }
            else
            {
                var img = SystemUtils.GetIcon(obj.Local);
                if (img == null)
                {
                    using var item = assm.GetManifestResourceStream("ColorDesktop.PGLauncherPlugin.Resource.image2.png")!;
                    Icon = new Bitmap(item);
                }
                else
                {
                    Icon = img;
                }
            }
        }
    }

    public void Launch()
    {
        if (string.IsNullOrWhiteSpace(_obj.Local))
        {
            return;
        }
        try
        {
            SystemUtils.Launch(_obj);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}