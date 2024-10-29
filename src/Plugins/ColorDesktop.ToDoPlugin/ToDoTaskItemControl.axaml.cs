using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace ColorDesktop.ToDoPlugin;

public partial class ToDoTaskItemControl : UserControl
{
    public ToDoTaskItemControl()
    {
        InitializeComponent();

        PointerEntered += ToDoTaskItemControl_PointerEntered;
        PointerExited += ToDoTaskItemControl_PointerExited;

        DataContextChanged += ToDoTaskItemControl_DataContextChanged;

        TextBox1.LostFocus += TextBox1_LostFocus;
        TextBox1.KeyDown += TextBox1_KeyDown;

        TextBox2.LostFocus += TextBox2_LostFocus;
        TextBox2.KeyDown += TextBox2_KeyDown;

        TextBox3.LostFocus += TextBox3_LostFocus;
        TextBox3.KeyDown += TextBox3_KeyDown;
    }

    private void TextBox3_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            if (DataContext is ToDoTaskItemModel model)
            {
                model.BodyEnd();
            }
        }
        else if (e.Key == Key.Escape)
        {
            if (DataContext is ToDoTaskItemModel model)
            {
                model.BodyCancel();
            }
        }
    }

    private void TextBox3_LostFocus(object? sender, RoutedEventArgs e)
    {
        if (DataContext is ToDoTaskItemModel model)
        {
            model.BodyEnd();
        }
    }

    private void TextBox2_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            if (DataContext is ToDoTaskItemModel model)
            {
                model.TitleEnd();
            }
        }
        else if (e.Key == Key.Escape)
        {
            if (DataContext is ToDoTaskItemModel model)
            {
                model.TitleCancel();
            }
        }
    }

    private void TextBox2_LostFocus(object? sender, RoutedEventArgs e)
    {
        if (DataContext is ToDoTaskItemModel model)
        {
            model.TitleEnd();
        }
    }

    private void TextBox1_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            if (DataContext is ToDoTaskItemModel model)
            {
                model.NewStepEnd();
            }
        }
        else if (e.Key == Key.Escape)
        {
            if (DataContext is ToDoTaskItemModel model)
            {
                model.NewStepCancel();
            }
        }
    }

    private void TextBox1_LostFocus(object? sender, RoutedEventArgs e)
    {
        if (DataContext is ToDoTaskItemModel model)
        {
            model.NewStepEnd();
        }
    }

    private void ToDoTaskItemControl_DataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is ToDoTaskItemModel model)
        {
            model.PropertyChanged += Model_PropertyChanged;
        }
    }

    private void Model_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == ToDoTaskItemModel.StepName)
        {
            TextBox1.Focus();
        }
        else if (e.PropertyName == ToDoTaskItemModel.TitleName)
        {
            TextBox2.Focus();
        }
        else if (e.PropertyName == ToDoTaskItemModel.BodyName)
        {
            TextBox3.Focus();
        }
    }

    private void ToDoTaskItemControl_PointerExited(object? sender, PointerEventArgs e)
    {
        if (DataContext is ToDoTaskItemModel model)
        {
            model.PointerOver(false);
        }
    }

    private void ToDoTaskItemControl_PointerEntered(object? sender, PointerEventArgs e)
    {
        if (DataContext is ToDoTaskItemModel model)
        {
            model.PointerOver(true);
        }
    }
}