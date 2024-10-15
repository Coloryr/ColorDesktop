using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.LogicalTree;

namespace ColorDesktop.CalendarPlugin.Skin1;

public partial class Skin1Control : UserControl
{
    public static readonly SelfPageSlideX PageSlide500 = new(TimeSpan.FromMilliseconds(500));

    private readonly Dictionary<int, MonthControl> _months = [];

    private CancellationTokenSource _cancel = new();

    private bool _switch1 = false;

    private int _now = -1;

    private CalendarModel _model;

    public Skin1Control()
    {
        InitializeComponent();

        DataContextChanged += Skin1Control_DataContextChanged;

        for (int a = 0; a < 12; a++)
        {
            _months.Add(a, new());
        }
    }

    protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromLogicalTree(e);

        _model.PropertyChanged -= Model_PropertyChanged;
        _model = null;
    }

    private void Skin1Control_DataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is CalendarModel model)
        {
            _model = model;

            if (model.NowMouth != 0)
            {
                Go(_months[model.NowMouth - 1]);
                _now = model.NowMouth;

                for (int a = 0; a < 12; a++)
                {
                    _months[a].DataContext = new MonthModel(model.NowYear, a + 1, model.TextColor, model.WeekStart);
                }
            }

            model.PropertyChanged += Model_PropertyChanged;
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