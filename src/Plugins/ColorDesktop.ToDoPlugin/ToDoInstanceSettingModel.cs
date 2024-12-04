using Avalonia.Controls;
using Avalonia.Media;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;
using ColorDesktop.CoreLib;
using ColorDesktop.ToDoPlugin.Net;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ColorDesktop.ToDoPlugin;

public partial class ToDoInstanceSettingModel : ObservableObject
{
    [ObservableProperty]
    private int _width;
    [ObservableProperty]
    private int _height;

    [ObservableProperty]
    private bool _isLogin;
    [ObservableProperty]
    private bool _loginNow;
    [ObservableProperty]
    private bool _displayText;
    [ObservableProperty]
    private bool _displayInfo;

    [ObservableProperty]
    private Color _backColor;
    [ObservableProperty]
    private Color _textColor;

    [ObservableProperty]
    private string _text;

    private readonly InstanceDataObj _obj;
    private readonly ToDoInstanceObj _config;
    public ToDoInstanceSettingModel(InstanceDataObj obj)
    {
        _obj = obj;
        _config = ToDoPlugin.GetConfig(obj);

        _width = _config.Width;
        _height = _config.Height;

        if (!Color.TryParse(_config.BackColor, out _backColor))
        {
            _backColor = Colors.Black;
        }
        if (!Color.TryParse(_config.TextColor, out _textColor))
        {
            _textColor = Colors.White;
        }

        _isLogin = _config.Token != null;
    }

    partial void OnTextChanged(string value)
    {
        _config.TextColor = value.ToString();
        ToDoPlugin.SaveConfig(_obj, _config);
    }

    partial void OnBackColorChanged(Color value)
    {
        _config.BackColor = value.ToString();
        ToDoPlugin.SaveConfig(_obj, _config);
    }

    partial void OnWidthChanged(int value)
    {
        _config.Width = value;
        ToDoPlugin.SaveConfig(_obj, _config);
    }

    partial void OnHeightChanged(int value)
    {
        _config.Height = value;
        ToDoPlugin.SaveConfig(_obj, _config);
    }

    [RelayCommand]
    public async Task Login(Control? control)
    {
        LoginNow = true;
        try
        {
            var res = await OAuth.GetCodeAsync();
            if (res.State == LoginState.Done)
            {
                Text = string.Format(LangApi.GetLang("ToDoInstanceSetting.Info1"), res.Message, res.Code);

                DisplayText = true;

                CoreHelper.OpUrl(res.Message);

                var top = TopLevel.GetTopLevel(control);
                if (top?.Clipboard != null)
                {
                    await top.Clipboard.SetTextAsync(res.Code);
                }

                var res1 = await OAuth.RunGetCodeAsync();
                if (res1 == null || res1.State != LoginState.Done)
                {
                    Text = LangApi.GetLang("ToDoInstanceSetting.Error3");

                    DisplayText = false;
                    DisplayInfo = true;
                }
                else
                {
                    _config.Token = res1.Obj!.AccessToken;
                    _config.RefreshToken = res1.Obj.RefreshToken;

                    ToDoPlugin.SaveConfig(_obj, _config);

                    Text = LangApi.GetLang("ToDoInstanceSetting.Info2");

                    DisplayText = false;
                    DisplayInfo = true;

                    IsLogin = true;
                }
            }
            else
            {
                Text = LangApi.GetLang("ToDoInstanceSetting.Error1");

                DisplayText = false;
                DisplayInfo = true;

                IsLogin = false;
            }
        }
        catch
        {
            Text = LangApi.GetLang("ToDoInstanceSetting.Error2");

            DisplayText = false;
            DisplayInfo = true;

            IsLogin = false;
        }
        LoginNow = false;
    }

    [RelayCommand]
    public void Cancel()
    {
        OAuth.Cancel();
    }

    [RelayCommand]
    public async Task Reload()
    {
        if (string.IsNullOrWhiteSpace(_config.RefreshToken))
        {
            return;
        }
        LoginNow = true;
        try
        {
            var res = await OAuth.RefreshTokenAsync(_config.RefreshToken);
            if (res.State == LoginState.Done)
            {
                _config.Token = res.Obj!.AccessToken;
                _config.RefreshToken = res.Obj.RefreshToken;

                ToDoPlugin.SaveConfig(_obj, _config);

                Text = LangApi.GetLang("ToDoInstanceSetting.Text6");

                DisplayText = false;
                DisplayInfo = true;
            }
            else
            {
                Text = LangApi.GetLang("ToDoInstanceSetting.Error5");

                DisplayText = false;
                DisplayInfo = true;

                IsLogin = false;
            }
        }
        catch
        {
            Text = LangApi.GetLang("ToDoInstanceSetting.Error4");

            DisplayText = false;
            DisplayInfo = true;

            IsLogin = false;
        }
        LoginNow = false;
    }
}
