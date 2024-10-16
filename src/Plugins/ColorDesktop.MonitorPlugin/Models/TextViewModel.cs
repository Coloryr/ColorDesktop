using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.MonitorPlugin.Models;

public partial class TextViewModel : MonitorItemModel
{
    public IBrush BackColor { get; init; }
    public IBrush TextColor { get; init; }

    [ObservableProperty]
    private string _text;

    public TextViewModel(MonitorItemObj item) : base(item)
    {
        BackColor = Brush.Parse(item.Color1);
        TextColor = Brush.Parse(item.Color2);
    }

    public override void Update()
    {
        base.Update();

        switch (ValueType)
        {
            case ValueType.Now:
                Text = Format;
                break;
            case ValueType.Max:
                Text = FormatMax;
                break;
            case ValueType.Min:
                Text = FormatMin;
                break;
        }
    }
}
