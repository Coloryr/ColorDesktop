using Avalonia.Media;
using ColorDesktop.Api;
using ColorDesktop.CoreLib;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ColorDesktop.OneWordPlugin;

public partial class OneWordModel : ObservableObject
{
    [ObservableProperty]
    private int _width;
    [ObservableProperty]
    private int _size;
    [ObservableProperty]
    private IBrush _textColor;
    [ObservableProperty]
    private IBrush _backColor;

    [ObservableProperty]
    private bool _showButton;
    [ObservableProperty]
    private bool _isUpdate;

    [ObservableProperty]
    private string _text;

    private DateTime _date;

    [RelayCommand]
    public async Task Update()
    {
        if (IsUpdate)
        {
            return;
        }

        try
        {
            var data = await HttpUtils.Client.GetStringAsync("https://xiaoapi.cn/API/yiyan.php");
            Text = data.Trim();
        }
        catch
        {
            Text = LangApi.GetLang("OneWordControl.Error1");
        }
    }

    public void Update(OneWordInstanceObj obj)
    {
        Width = obj.Width;
        Size = obj.Size;
        try
        {
            TextColor = Brush.Parse(obj.TextColor);
            BackColor = Brush.Parse(obj.BackColor);
        }
        catch
        { 
            
        }
    }

    public async void Tick()
    {
        var now = DateTime.Now;
        if (now.DayOfYear != _date.DayOfYear)
        {
            _date = now;
            await Update();
        }
    }
}
