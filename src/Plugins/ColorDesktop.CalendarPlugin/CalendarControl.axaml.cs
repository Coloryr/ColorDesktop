using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using ColorDesktop.Api;

namespace ColorDesktop.CalendarPlugin;

public partial class CalendarControl : UserControl, IInstance
{
    public static readonly SelfPageSlideX PageSlide500 = new(TimeSpan.FromMilliseconds(500));

    private readonly Dictionary<int, MonthControl> _months = [];

    private CancellationTokenSource _cancel = new();

    private bool _switch1 = false;

    private int _now = -1;

    public CalendarControl()
    {
        InitializeComponent();

        var model = new CalendarModel();
        model.PropertyChanged += Model_PropertyChanged;
        DataContext = model;

        PointerEntered += CalendarControl_PointerEntered;
        PointerExited += CalendarControl_PointerExited;

        for (int a = 0; a < 12; a++)
        {
            _months.Add(a, new());
        }
    }

    private void Model_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(CalendarModel.NowMouth))
        {
            var model = (DataContext as CalendarModel)!;
            Go(_months[model.NowMouth - 1]);
            _now = model.NowMouth;
        }
        else if (e.PropertyName == CalendarModel.LoadMonthName)
        {
            var model = (DataContext as CalendarModel)!;
            for (int a = 0; a < 12; a++)
            {
                _months[a].DataContext = new MonthModel(model.NowYear, a + 1, model.TextColor, model.WeekStart);
            }
        }
    }

    private void CalendarControl_PointerExited(object? sender, PointerEventArgs e)
    {
        if (DataContext is CalendarModel model)
        {
            model.ShowButton = false;
        }
    }

    private void CalendarControl_PointerEntered(object? sender, PointerEventArgs e)
    {
        if (DataContext is CalendarModel model)
        {
            model.ShowButton = true;
        }
    }

    public Control CreateView()
    {
        return this;
    }

    public void RenderTick()
    {
        if (DataContext is CalendarModel model)
        {
            model.Tick();
        }
    }

    public void Start()
    {
        
    }

    public void Stop()
    {
        
    }

    public void Update(InstanceDataObj obj)
    {
        var config = CalendarPlugin.GetConfig(obj);
        if (DataContext is CalendarModel model)
        {
            model.Update(config);
        }
    }

    private void Go(Control to)
    {
        _cancel.Cancel();
        _cancel.Dispose();

        _cancel = new();

        var model = (DataContext as CalendarModel)!;

        if (_now == -1)
        {
            Content1.Child = to;
            return;
        }

        SwitchTo(_switch1, to, _now < model.NowMouth, _cancel.Token);

        _switch1 = !_switch1;
    }

    public void SwitchTo(bool dir, Control control, bool dir1, CancellationToken token)
    {
        if (!dir)
        {
            Content2.Child = control;
            PageSlide500.Start(Content1, Content2, dir1, token);
        }
        else
        {
            Content1.Child = control;
            PageSlide500.Start(Content2, Content1, dir1, token);
        }
    }
}