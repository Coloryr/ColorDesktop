using System.ComponentModel;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using Avalonia.Rendering;
using Avalonia.Threading;
using ColorDesktop.CoreLib;
using MinecraftSkinRender;
using MinecraftSkinRender.MojangApi;
using MinecraftSkinRender.OpenGL;
using Newtonsoft.Json;
using SkiaSharp;

namespace ColorDesktop.MinecraftSkinPlugin;

public class SkinRender : OpenGlControlBase, ICustomHitTest
{
    private SkinRenderOpenGL skin;
    private DateTime time;

    private bool _needUpdate;
    private Func<Task> _run;

    public int Fps;

    public SkinRender()
    {
        DataContextChanged += SkinRender_DataContextChanged;

        PointerWheelChanged += OpenGlPageControl_PointerWheelChanged;
        PointerPressed += OpenGlPageControl_PointerPressed;
        PointerReleased += OpenGlPageControl_PointerReleased;
        PointerMoved += OpenGlPageControl_PointerMoved;
    }

    private void SkinRender_DataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is MinecraftSkinModel model)
        {
            model.PropertyChanged += Model_PropertyChanged;
        }
    }

    private void Model_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        var model = (DataContext as MinecraftSkinModel)!;
        if (e.PropertyName == MinecraftSkinModel.RotateName)
        {
            Dispatcher.UIThread.Post(() =>
            {
                skin.ArmRotate = model.ArmRotate;
                skin.LegRotate = model.LegRotate;
                skin.HeadRotate = model.HeadRotate;
            });
        }
        else if (e.PropertyName == MinecraftSkinModel.PosName)
        {
            skin.Pos(model.X, model.Y);
        }
        else if (e.PropertyName == MinecraftSkinModel.ScollName)
        {
            skin.AddDis(model.X);
        }
        else if (e.PropertyName == MinecraftSkinModel.RotName)
        {
            skin.Rot(model.X, model.Y);
        }
        else if (e.PropertyName == MinecraftSkinModel.LoadName)
        {
            Update();
        }
        else if (e.PropertyName == MinecraftSkinModel.ResetName)
        {
            skin.ResetPos();
        }
    }

    public void Update()
    {
        _needUpdate = true;
    }

    public void Update(SkinInstanceObj config)
    {
        _run = () =>
        {
            return UpdateTask(config);
        };

        Update();
    }

    private async Task UpdateTask(SkinInstanceObj config)
    {
        skin.Animation = config.EnableAnimation;
        skin.EnableCape = config.EnableCape;
        skin.EnableTop = config.EnableTop;
        skin.RenderType = config.EnableMSAA ? SkinRenderType.MSAA : SkinRenderType.Normal;

        switch (config.FileType)
        {
            case FileType.Name:
                try
                {
                    var res = await MinecraftAPI.GetMinecraftProfileNameAsync(config.File);
                    if (res == null)
                    {
                        return;
                    }
                    var res1 = await MinecraftAPI.GetUserProfile(res!.UUID);
                    TexturesObj? obj = null;
                    foreach (var item in res1!.properties)
                    {
                        if (item.name == "textures")
                        {
                            var temp = Convert.FromBase64String(item.value);
                            var data = Encoding.UTF8.GetString(temp);
                            obj = JsonConvert.DeserializeObject<TexturesObj>(data);
                            break;
                        }
                    }
                    if (obj == null)
                    {
                        return;
                    }
                    if (obj!.textures.SKIN.url != null)
                    {
                        try
                        {
                            var img = await TempManager.LoadSkImage(obj!.textures.SKIN.url);
                            skin.SetSkinTex(img);
                        }
                        catch
                        {

                        }
                    }
                    if (obj.textures.CAPE.url != null)
                    {
                        try
                        {
                            var img = await TempManager.LoadSkImage(obj!.textures.CAPE.url);
                            skin.SetCapeTex(img);
                        }
                        catch
                        {

                        }
                    }
                }
                catch
                {

                }
                break;
            case FileType.Url:
                if (!string.IsNullOrWhiteSpace(config.File) && config.File.StartsWith("http"))
                {
                    try
                    {
                        var img = await TempManager.LoadSkImage(config.File);
                        skin.SetSkinTex(img);
                    }
                    catch
                    {

                    }
                }
                if (!string.IsNullOrWhiteSpace(config.File1) && config.File1.StartsWith("http"))
                {
                    try
                    {
                        var img = await TempManager.LoadSkImage(config.File1);
                        skin.SetCapeTex(img);
                    }
                    catch
                    {

                    }
                }
                break;
            case FileType.LocalFile:
                if (!string.IsNullOrWhiteSpace(config.File) && config.File.EndsWith(".png")
                    && File.Exists(config.File))
                {
                    try
                    {
                        var img = SKBitmap.Decode(config.File);
                        skin.SetSkinTex(img);
                    }
                    catch
                    {

                    }
                }
                if (!string.IsNullOrWhiteSpace(config.File1) && config.File1.EndsWith(".png")
                    && File.Exists(config.File1))
                {
                    try
                    {
                        var img = SKBitmap.Decode(config.File1);
                        skin.SetCapeTex(img);
                    }
                    catch
                    {

                    }
                }
                break;
        }

        skin.SkinType = config.SkinType;

        if (DataContext is MinecraftSkinModel model)
        {
            model.HaveSkin = skin.HaveSkin;
        }
    }

    public void Reset()
    {
        skin.ResetPos();

        RequestNextFrameRendering();
    }

    private void OpenGlPageControl_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (skin == null || !skin.HaveSkin)
        {
            return;
        }

        var po = e.GetCurrentPoint(this);
        var pos = e.GetPosition(this);

        KeyType type = KeyType.None;
        if (po.Properties.IsLeftButtonPressed)
        {
            type = KeyType.Left;
        }
        else if (po.Properties.IsRightButtonPressed)
        {
            type = KeyType.Right;
        }

        skin.PointerPressed(type, new((float)pos.X, (float)pos.Y));

        RequestNextFrameRendering();
    }

    private void OpenGlPageControl_PointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (skin == null || !skin.HaveSkin)
        {
            return;
        }

        var po = e.GetCurrentPoint(this);
        var pos = e.GetPosition(this);

        KeyType type = KeyType.None;
        if (po.Properties.IsLeftButtonPressed)
        {
            type = KeyType.Left;
        }
        else if (po.Properties.IsRightButtonPressed)
        {
            type = KeyType.Right;
        }

        skin.PointerReleased(type, new((float)pos.X, (float)pos.Y));

        RequestNextFrameRendering();
    }

    private void OpenGlPageControl_PointerMoved(object? sender, PointerEventArgs e)
    {
        if (skin == null || !skin.HaveSkin)
        {
            return;
        }

        var po = e.GetCurrentPoint(this);
        var pos = e.GetPosition(this);

        KeyType type = KeyType.None;
        if (po.Properties.IsLeftButtonPressed)
        {
            type = KeyType.Left;
        }
        else if (po.Properties.IsRightButtonPressed)
        {
            type = KeyType.Right;
        }

        skin.PointerMoved(type, new((float)pos.X, (float)pos.Y));

        RequestNextFrameRendering();
    }

    private void OpenGlPageControl_PointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {
        if (skin == null || !skin.HaveSkin)
        {
            return;
        }

        skin.PointerWheelChanged(e.Delta.Y > 0);

        RequestNextFrameRendering();
    }

    protected override unsafe void OnOpenGlInit(GlInterface gl)
    {
        CheckError(gl);

        skin = new(new AvaloniaApi(gl))
        {
            IsGLES = GlVersion.Type == GlProfileType.OpenGLES,
            BackColor = new(0, 0, 0, 0)
        };
        skin.OpenGlInit();
    }

    protected override unsafe void OnOpenGlRender(GlInterface gl, int fb)
    {
        if (_needUpdate)
        {
            _needUpdate = false;
            _run();
        }

        int x = (int)Bounds.Width;
        int y = (int)Bounds.Height;

        if (VisualRoot is TopLevel window)
        {
            var screen = window.RenderScaling;
            x = (int)(Bounds.Width * screen);
            y = (int)(Bounds.Height * screen);
        }

        skin.Width = x;
        skin.Height = y;

        if (time.Ticks == 0)
        {
            time = DateTime.Now;
        }

        var time1 = DateTime.Now;
        var temp = time1 - time;
        time = time1;

        skin.Tick(temp.TotalSeconds);
        skin.OpenGlRender(fb);

        Fps++;

        CheckError(gl);
    }

    private static void CheckError(GlInterface gl)
    {
        int err;
        while ((err = gl.GetError()) != GlConsts.GL_NO_ERROR)
            Console.WriteLine(err);
    }

    protected override void OnOpenGlDeinit(GlInterface gl)
    {
        skin.OpenGlDeinit();
    }

    public bool HitTest(Point point)
    {
        return Bounds.Contains(point);
    }
}