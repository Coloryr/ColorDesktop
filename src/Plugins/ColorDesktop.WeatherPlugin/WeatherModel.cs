using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using ColorDesktop.WeatherPlugin.Objs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ColorDesktop.WeatherPlugin;

public partial class WeatherModel : ObservableObject
{
    [ObservableProperty]
    private string _value;
    [ObservableProperty]
    private string _city;
    [ObservableProperty]
    private Bitmap? _img;
    [ObservableProperty]
    private string _state;

    [ObservableProperty]
    private IBrush _backColor;
    [ObservableProperty]
    private IBrush _textColor;

    [ObservableProperty]
    private bool _error;

    [ObservableProperty]
    private bool _isUpdate;

    private CityObj? _obj;

    private bool _test;

    [RelayCommand]
    public async Task Update()
    {
        if (_obj == null)
        {
            Error = true;
            return;
        }
        
        IsUpdate = true;
        var data = await WebClient.GetNow(_obj);
        if (data?.Results.FirstOrDefault() is { } res)
        {
            Error = false;
            Value = res.Now.Temperature;
            City = res.Location.Name;
            State = res.Now.Text;

            var img1 = Img;

            var assm = Assembly.GetExecutingAssembly();
            using var item = assm.GetManifestResourceStream($"ColorDesktop.WeatherPlugin.Resource.Pics.{res.Now.Code}@2x.png")!;
            Img = new Bitmap(item);

            img1?.Dispose();
        }
        else
        {
            Error = true;
        }
        IsUpdate = false;
       
    }

    public async void Tick()
    {
        if (_test)
        {
            return;
        }
        _test = true;

        await Update();
    }

    public void Update(WeatherInstanceObj obj)
    {
        BackColor = Brush.Parse(obj.BackColor);
        TextColor = Brush.Parse(obj.TextColor);
        _obj = WeatherPlugin.GetCity(obj.City);
    }
}
