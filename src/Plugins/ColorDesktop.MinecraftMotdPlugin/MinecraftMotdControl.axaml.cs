using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;
using ColorDesktop.MinecraftMotdPlugin.Motd;

namespace ColorDesktop.MinecraftMotdPlugin;

public partial class MinecraftMotdControl : UserControl, IInstance
{
    public static IBrush GetColor(string? color)
    {
        if (string.IsNullOrWhiteSpace(color))
        {
            return Brushes.White;
        }
        if (color.StartsWith('#'))
        {
            return Brush.Parse(color);
        }
        if (ServerMotd.ColorMap.TryGetValue(color, out var color1))
        {
            return Brush.Parse(color1);
        }

        return Brush.Parse(color);
    }

    private bool _firstLine = true;
    private readonly Random _random = new();

    private string _ip;
    private ushort _port;

    public MinecraftMotdControl()
    {
        InitializeComponent();

        Button2.Click += Button2_Click;
    }

    public Control CreateView()
    {
        return this;
    }

    public void RenderTick(IInstanceWindow window)
    {

    }

    public void Start(IInstanceWindow window)
    {

    }

    public void Stop(IInstanceWindow window)
    {

    }

    public void Update(InstanceDataObj obj)
    {
        var config = MinecraftMotdPlugin.GetConfig(obj);
        _ip = config.IP;
        _port = config.Port ?? 25565;

        Update();
    }

    private void Button2_Click(object? sender, RoutedEventArgs e)
    {
        Update();
    }

    private async void Update()
    {
        Button2.IsVisible = false;

        Grid1.IsVisible = true;

        _firstLine = true;

        StackPanel1.Inlines?.Clear();
        StackPanel2.Inlines?.Clear();
        StackPanel1.Text = "";
        StackPanel2.Text = "";

        var ip = _ip;
        var port = _port;
        if (ip == null)
        {
            Button2.IsVisible = true;
            return;
        }

        var motd = await Task.Run(() =>
        {
            return ServerMotd.GetServerInfo(ip, port);
        });
        if (motd.State == StateType.Ok)
        {
            Grid2.IsVisible = false;
            using var stream = new MemoryStream(motd.FaviconByteArray);
            Image1.Source = new Bitmap(stream);

            Label2.Text = motd.Players?.Online.ToString();
            Label3.Text = motd.Players?.Max.ToString();
            Label4.Text = motd.Version?.Name;
            Label5.Text = motd.Ping.ToString();

            MakeText(motd.Description);

            StackPanel1.Inlines?.Add(new Run(""));
            StackPanel2.Inlines?.Add(new Run(""));
        }
        else
        {
            Grid2.IsVisible = true;
        }
        Grid1.IsVisible = false;

        Button2.IsVisible = true;
    }

    public void MakeText(ChatObj chat)
    {
        if (chat.Text == "\n")
        {
            _firstLine = false;
            return;
        }

        if (!string.IsNullOrWhiteSpace(chat.Text))
        {
            var text = new Run()
            {
                Text = chat.Obfuscated ? " " : chat.Text,
                Foreground = GetColor(chat.Color)
            };

            if (chat.Bold)
            {
                text.FontWeight = FontWeight.Bold;
            }
            if (chat.Italic)
            {
                text.FontStyle = FontStyle.Oblique;
            }
            if (chat.Underlined)
            {
                if (text.TextDecorations == null)
                {
                    text.TextDecorations = TextDecorations.Underline;
                }
                else
                {
                    text.TextDecorations.AddRange(TextDecorations.Underline);
                }
            }
            if (chat.Strikethrough)
            {
                if (text.TextDecorations == null)
                {
                    text.TextDecorations = TextDecorations.Strikethrough;
                }
                else
                {
                    text.TextDecorations.AddRange(TextDecorations.Strikethrough);
                }
            }

            if (chat.Obfuscated)
            {
                text.Text = new string((char)_random.Next(33, 126), 1);
            }

            AddText(text);
        }

        if (chat.Extra != null)
        {
            foreach (var item in chat.Extra)
            {
                MakeText(item);
            }
        }
    }

    public void AddText(Inline text)
    {
        if (_firstLine)
        {
            StackPanel1.Inlines?.Add(text);
        }
        else
        {
            StackPanel2.Inlines?.Add(text);
        }
    }

    public IInstanceHandel? GetHandel()
    {
        return null;
    }

    public void WindowLoaded(IInstanceWindow window)
    {

    }
}