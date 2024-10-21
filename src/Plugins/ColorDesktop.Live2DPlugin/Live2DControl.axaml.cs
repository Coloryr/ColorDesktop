using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using Live2DCSharpSDK.App;
using Live2DCSharpSDK.OpenGL;

namespace ColorDesktop.Live2DPlugin;

public partial class Live2DControl : UserControl
{
    public Live2DControl()
    {
        InitializeComponent();
    }
}

public class OpenGlPageControl : OpenGlControlBase
{
    private LAppDelegate lapp;

    private string _info = string.Empty;
    private DateTime time;

    public static readonly DirectProperty<OpenGlPageControl, string> InfoProperty =
        AvaloniaProperty.RegisterDirect<OpenGlPageControl, string>("Info", o => o.Info, (o, v) => o.Info = v);

    public OpenGlPageControl()
    {

    }

    public string Info
    {
        get => _info;
        private set => SetAndRaise(InfoProperty, ref _info, value);
    }

    private static void CheckError(GlInterface gl)
    {
        int err;
        while ((err = gl.GetError()) != GlConsts.GL_NO_ERROR)
            Console.WriteLine(err);
    }

    protected override unsafe void OnOpenGlInit(GlInterface gl)
    {
        CheckError(gl);

        Info = $"Renderer: {gl.GetString(GlConsts.GL_RENDERER)} Version: {gl.GetString(GlConsts.GL_VERSION)}";

        try
        {
            lapp = new LAppDelegateOpenGL(new AvaloniaApi(this, gl))
            {
                BGColor = new(0, 0, 0, 0)
            };
        }
        catch (Exception e)
        {

        }
        var model = lapp.Live2dManager.LoadModel("F:\\live2d\\Resources\\Haru\\", "Haru");
        CheckError(gl);
    }

    protected override void OnOpenGlDeinit(GlInterface GL)
    {
        lapp.Dispose();
    }

    protected override void OnOpenGlRender(GlInterface gl, int fb)
    {
        int x = (int)Bounds.Width;
        int y = (int)Bounds.Height;

        if (VisualRoot is TopLevel window)
        {
            var screen = window.RenderScaling;
            x = (int)(Bounds.Width * screen);
            y = (int)(Bounds.Height * screen);
        }
        gl.Viewport(0, 0, x, y);
        var now = DateTime.Now;
        float span = 0;
        if (time.Ticks == 0)
        {
            time = now;
        }
        else
        {
            span = (float)(now - time).TotalSeconds;
            time = now;
        }
        lapp.Run(span);
        CheckError(gl);
    }
}