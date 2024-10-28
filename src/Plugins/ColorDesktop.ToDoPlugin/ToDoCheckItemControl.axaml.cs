using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace ColorDesktop.ToDoPlugin;

public partial class ToDoCheckItemControl : UserControl
{
    public ToDoCheckItemControl()
    {
        InitializeComponent();

        DataContextChanged += ToDoCheckItemControl_DataContextChanged;

        Text1.KeyDown += Text1_KeyDown;
        Text1.LostFocus += Text1_LostFocus;
    }

    private void Text1_LostFocus(object? sender, RoutedEventArgs e)
    {
        if (DataContext is ToDoCheckItemItemModel model)
        {
            model.EditEnd();
        }
    }

    private void Text1_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            if (DataContext is ToDoCheckItemItemModel model)
            {
                model.EditEnd();
            }
        }
        else if (e.Key == Key.Escape)
        {
            if (DataContext is ToDoCheckItemItemModel model)
            {
                model.Cancel();
            }
        }
    }

    private void ToDoCheckItemControl_DataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is ToDoCheckItemItemModel model)
        {
            model.PropertyChanged += Model_PropertyChanged;
        }
    }

    private void Model_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == ToDoCheckItemItemModel.EditName)
        {
            Text1.Focus();
        }
    }
}