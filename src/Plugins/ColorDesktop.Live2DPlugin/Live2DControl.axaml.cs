using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using Avalonia.Rendering;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;
using Live2DCSharpSDK.App;
using Live2DCSharpSDK.Framework.Motion;
using Live2DCSharpSDK.OpenGL;

namespace ColorDesktop.Live2DPlugin;

public partial class Live2DControl : UserControl, IInstance
{
    private bool _lowFps = false;
    private bool _lastTick = false;
    private DateTime _time;

    public Live2DControl()
    {
        InitializeComponent();
    }

    public Control CreateView()
    {
        return this;
    }

    public void RenderTick(IInstanceWindow window)
    {
        if (_time.Ticks == 0)
        {
            _time = DateTime.Now;
        }
        else
        {
            var time = DateTime.Now;
            var less = time - _time;
            if (less.TotalSeconds > 1)
            {
                _time = time;
                Fps.Text = View1.Fps.ToString();
                View1.Fps = 0;
            }
        }
        if (_lowFps)
        {
            if (!_lastTick)
            {
                View1.RequestNextFrameRendering();
            }
            _lastTick = !_lastTick;
        }
        else
        {
            View1.RequestNextFrameRendering();
        }
    }

    public void Start(IInstanceWindow window)
    {

    }

    public void Stop(IInstanceWindow window)
    {

    }

    public void Update(InstanceDataObj obj)
    {
        var config = Live2DPlugin.GetConfig(obj);
        View1.Update(config);
        Width = config.Width;
        Height = config.Height;
        _lowFps = config.LowFps;
        FpsView.IsVisible = config.DisplayFps;

        Live2DPlugin.AddView(obj.UUID, View1);
    }

    public IInstanceHandel? GetHandel()
    {
        return null;
    }

    public void WindowLoaded(IInstanceWindow window)
    {
        RenderTick(window);
    }
}

public class OpenGlPageControl : OpenGlControlBase, ICustomHitTest
{
    private static void CheckError(GlInterface gl)
    {
        int err;
        while ((err = gl.GetError()) != GlConsts.GL_NO_ERROR)
            Console.WriteLine(err);
    }

    private static List<MenuItem> GenModelMotionFlyout(LAppModel model)
    {
        var list = new List<MenuItem>();
        var list1 = model.Motions;
        if (list1.Count != 0)
        {
            list1.ForEach(item =>
            {
                var button = new MenuItem()
                {
                    Header = item
                };
                button.Click += (a, b) =>
                {
                    model.StartMotion(item, MotionPriority.PriorityForce);
                };
                list.Add(button);
            });
        }

        return list;
    }

    private static List<MenuItem> GenModelExpressionFlyout(LAppModel model)
    {
        var list = new List<MenuItem>();
        var list1 = model.Expressions;
        if (list1.Count != 0)
        {
            list1.ForEach(item =>
            {
                var button = new MenuItem()
                {
                    Header = item
                };
                button.Click += (a, b) =>
                {
                    model.SetExpression(item);
                };
                list.Add(button);
            });
        }

        return list;
    }

    private static List<MenuItem> GenModelFlyout(LAppModel model)
    {
        var list1 = GenModelMotionFlyout(model);
        var list2 = GenModelExpressionFlyout(model);

        var list3 = new List<MenuItem>();
        if (list1.Count > 0)
        {
            list3.Add(new MenuItem()
            {
                Header = LangApi.GetLang("Live2DControl.Text1"),
                ItemsSource = list1
            });
        }
        if (list2.Count > 0)
        {
            list3.Add(new MenuItem()
            {
                Header = LangApi.GetLang("Live2DControl.Text2"),
                ItemsSource = list2
            });
        }

        if (list3.Count == 0)
        {
            list3.Add(new MenuItem()
            {
                Header = LangApi.GetLang("Live2DControl.Text3"),
                IsEnabled = false
            });
        }

        return list3;
    }

    private LAppDelegate _lapp;

    private DateTime time;

    private readonly Dictionary<(string, string), LAppModel> _models = [];

    private bool _runUpdate;
    private Action _run;

    public int Fps;

    public OpenGlPageControl()
    {
        PointerPressed += Live2dTop_PointerPressed;
        PointerReleased += Live2dTop_PointerReleased;
        PointerMoved += Live2dTop_PointerMoved;
    }

    public void Update()
    {
        _runUpdate = true;
    }

    public void Update(Live2DInstanceObj config)
    {
        _runUpdate = true;
        _run = () =>
        {
            foreach (var item in config.Models)
            {
                bool isnew = true;
                foreach (var item1 in _models.ToArray())
                {
                    if (item1.Key.Item1 == item.Name
                        && item1.Key.Item2 == item.Local)
                    {
                        item1.Value.ModelMatrix.Scale(item.Scale, item.Scale);
                        item1.Value.ModelMatrix.Translate(item.X, item.Y);
                        isnew = false;
                    }
                    else if (item1.Key.Item2 == item.Local)
                    {
                        item1.Value.ModelMatrix.Scale(item.Scale, item.Scale);
                        item1.Value.ModelMatrix.Translate(item.X, item.Y);
                        _models.Remove(item1.Key);
                        _models.Add((item.Name, item.Local), item1.Value);
                        isnew = false;
                    }
                    else if (item1.Key.Item1 == item.Name)
                    {
                        _models.Remove(item1.Key);
                        _lapp.Live2dManager.RemoveModel(item1.Value);
                        var info = new FileInfo(item.Local);
                        var model = _lapp.Live2dManager.LoadModel(info.DirectoryName!, info.Name);
                        model.ModelMatrix.Scale(item.Scale, item.Scale);
                        model.ModelMatrix.Translate(item.X, item.Y);
                        _models.Add((item.Name, item.Local), model);
                        isnew = false;
                    }
                    else
                    {
                        _models.Remove(item1.Key);
                        _lapp.Live2dManager.RemoveModel(item1.Value);
                        isnew = false;
                    }
                }
                if (isnew)
                {
                    var info = new FileInfo(item.Local);
                    var model = _lapp.Live2dManager.LoadModel(info.DirectoryName!, info.Name);
                    model.ModelMatrix.Scale(item.Scale, item.Scale);
                    model.ModelMatrix.Translate(item.X, item.Y);
                    _models.Add((item.Name, item.Local), model);
                }
            }
        };
    }

    private void Live2dTop_PointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        Release();
    }

    private void Live2dTop_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var pro = e.GetCurrentPoint(this);
        if (pro.Properties.IsLeftButtonPressed)
        {
            Pressed();
            Moved((float)pro.Position.X, (float)pro.Position.Y);
        }
        else if (pro.Properties.IsRightButtonPressed)
        {
            Flyout();
        }
    }

    private void Live2dTop_PointerMoved(object? sender, PointerEventArgs e)
    {
        var pro = e.GetCurrentPoint(this);
        if (pro.Properties.IsLeftButtonPressed)
            Moved((float)pro.Position.X, (float)pro.Position.Y);
    }

    private void Pressed()
    {
        _lapp.OnMouseCallBack(true);
    }

    private void Release()
    {
        _lapp.OnMouseCallBack(false);
    }

    private void Moved(float x, float y)
    {
        _lapp.OnMouseCallBack(x, y);
    }

    private void Flyout()
    {
        var flyout = new MenuFlyout();
        var list1 = new List<MenuItem>();
        foreach (var item in _models)
        {
            var button = new MenuItem()
            {
                Header = item.Key.Item1,
                ItemsSource = GenModelFlyout(item.Value)
            };

            list1.Add(button);
        }
        flyout.ItemsSource = list1;
        flyout.ShowAt(this, true);
    }

    protected override unsafe void OnOpenGlInit(GlInterface gl)
    {
        CheckError(gl);

        try
        {
            _lapp = new LAppDelegateOpenGL(new AvaloniaApi(this, gl))
            {
                BGColor = new(0, 0, 0, 0)
            };
        }
        catch (Exception e)
        {

        }
        CheckError(gl);
    }

    protected override void OnOpenGlDeinit(GlInterface GL)
    {
        _lapp.Dispose();
    }

    protected override void OnOpenGlRender(GlInterface gl, int fb)
    {
        if (_runUpdate)
        {
            _runUpdate = false;
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
        _lapp.Run(span);
        Fps++;
        CheckError(gl);
    }

    public bool HitTest(Point point)
    {
        return Bounds.Contains(point);
    }
}