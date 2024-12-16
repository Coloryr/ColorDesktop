using System.Reflection;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.PGLauncherPlugin;

public partial class PGItemModel : ObservableObject
{
    [ObservableProperty]
    private string _name;
    [ObservableProperty]
    private int _size;
    [ObservableProperty]
    private int _textSize;

    public Thickness Thickness => Demo ? new(0) : new(_obj.Margin.Left, _obj.Margin.Top, _obj.Margin.Right, _obj.Margin.Bottom);
    [ObservableProperty]
    private Thickness _border;

    [ObservableProperty]
    private IBrush _backColor;
    [ObservableProperty]
    private IBrush _textColor;

    [ObservableProperty]
    private bool _displayIcon;
    [ObservableProperty]
    private bool _displayText;
    public Bitmap Icon { get; init; }

    [ObservableProperty]
    private bool _isOver;

    private readonly PGItemObj _obj;

    public bool Demo = false;

    public PGItemModel(PGItemObj obj)
    {
        _obj = obj;

        _size = _obj.Size;
        _textSize = _obj.TextSize;
        try
        {
            _backColor = Brush.Parse(_obj.BackColor);
            _textColor = Brush.Parse(_obj.TextColor);
        }
        catch
        { 
            
        }
        _name = _obj.Name;
        _border = new(_obj.BorderSize);

        DisplayChange();

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

    public void DisplayChange()
    {
        switch (_obj.Display)
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
    }

    public void Launch()
    {
        if (Demo || string.IsNullOrWhiteSpace(_obj.Local))
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
