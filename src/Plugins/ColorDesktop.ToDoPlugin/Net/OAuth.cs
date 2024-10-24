using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorDesktop.ToDoPlugin.Objs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ColorDesktop.ToDoPlugin.Net;

public static class OAuth
{
    public const string OAuthCode = "https://login.microsoftonline.com/consumers/oauth2/v2.0/devicecode";
    public const string OAuthToken = "https://login.microsoftonline.com/consumers/oauth2/v2.0/token";

    public static readonly Dictionary<string, string> Arg1 = new()
    {
        { "client_id", "d4b143ea-c5f9-47cb-ad92-fc759f341b92" },
        { "scope", "offline_access Tasks.ReadWrite" }
    };

    public static readonly Dictionary<string, string> Arg2 = new()
    {
        { "client_id", "d4b143ea-c5f9-47cb-ad92-fc759f341b92" },
        { "grant_type", "urn:ietf:params:oauth:grant-type:device_code" },
        { "code", "" }
    };

    public static readonly Dictionary<string, string> Arg3 = new()
    {
        { "client_id", "d4b143ea-c5f9-47cb-ad92-fc759f341b92" },
        { "grant_type", "refresh_token" },
        { "refresh_token", "" }
    };

    private static string s_code;
    private static string s_url;
    private static string s_deviceCode;
    private static int s_expiresIn;
    private static CancellationTokenSource s_cancel;

    public static readonly HttpClient Client = new();

    /// <summary>
    /// 请求数据
    /// </summary>
    /// <param name="url">网址</param>
    /// <param name="arg">参数</param>
    /// <returns>数据</returns>
    public static async Task<string> LoginPostStringAsync(string url, Dictionary<string, string> arg)
    {
        FormUrlEncodedContent content = new(arg);
        using var message = await Client.PostAsync(url, content);

        return await message.Content.ReadAsStringAsync();
    }

    /// <summary>
    /// 请求数据
    /// </summary>
    /// <param name="url">网址</param>
    /// <param name="arg">参数</param>
    /// <returns>数据</returns>
    public static async Task<JObject?> LoginPostAsync(string url, Dictionary<string, string> arg)
    {
        var content = new FormUrlEncodedContent(arg);
        using var message = await Client.PostAsync(url, content);
        var data = await message.Content.ReadAsStringAsync();
        return JObject.Parse(data);
    }

    /// <summary>
    /// 获取登录码
    /// </summary>
    public static async Task<OAuthGetCodeRes> GetCodeAsync()
    {
        var data = await LoginPostStringAsync(OAuthCode, Arg1);
        if (data.Contains("error"))
        {
            return new OAuthGetCodeRes
            {
                State = LoginState.Error,
                Message = data
            };
        }
        var obj1 = JsonConvert.DeserializeObject<OAuthObj>(data);
        if (obj1 == null)
        {
            return new OAuthGetCodeRes
            {
                State = LoginState.DataError,
                Message = data
            };
        }
        s_code = obj1.UserCode;
        s_url = obj1.VerificationUri;
        s_deviceCode = obj1.DeviceCode;
        s_expiresIn = obj1.ExpiresIn;

        return new OAuthGetCodeRes
        {
            State = LoginState.Done,
            Code = s_code,
            Message = s_url
        };
    }

    /// <summary>
    /// 获取token
    /// </summary>
    public static async Task<OAuthGetTokenRes> RunGetCodeAsync()
    {
        Arg2["code"] = s_deviceCode;
        long startTime = DateTime.Now.Ticks;
        int delay = 2;
        s_cancel = new();
        do
        {
            await Task.Delay(delay * 1000);
            if (s_cancel.IsCancellationRequested)
            {
                return new OAuthGetTokenRes
                {
                    State = LoginState.Error
                };
            }
            long estimatedTime = DateTime.Now.Ticks - startTime;
            long sec = estimatedTime / 10000000;
            if (sec > s_expiresIn)
            {
                return new OAuthGetTokenRes
                {
                    State = LoginState.TimeOut
                };
            }
            var data = await LoginPostStringAsync(OAuthToken, Arg2);
            var obj3 = JObject.Parse(data);
            if (obj3 == null)
            {
                return new OAuthGetTokenRes
                {
                    State = LoginState.DataError
                };
            }
            if (obj3.ContainsKey("error"))
            {
                string? error = obj3["error"]?.ToString();
                if (error == "authorization_pending")
                {
                    continue;
                }
                else if (error == "slow_down")
                {
                    delay += 5;
                }
                else if (error == "expired_token")
                {
                    return new OAuthGetTokenRes
                    {
                        State = LoginState.Error
                    };
                }
            }
            else
            {
                var obj4 = JsonConvert.DeserializeObject<OAuthGetCodeObj>(data);
                if (obj4 == null)
                {
                    return new OAuthGetTokenRes
                    {
                        State = LoginState.DataError
                    };
                }

                return new OAuthGetTokenRes
                {
                    State = LoginState.Done,
                    Obj = obj4
                };
            }
        } while (true);
    }

    /// <summary>
    /// 刷新密匙
    /// </summary>
    public static async Task<OAuthGetTokenRes> RefreshTokenAsync(string token)
    {
        var dir = new Dictionary<string, string>(Arg3)
        {
            ["refresh_token"] = token
        };

        var obj1 = await LoginPostAsync(OAuthToken, dir);
        if (obj1 == null)
        {
            return new OAuthGetTokenRes
            {
                State = LoginState.DataError
            };
        }
        if (obj1.ContainsKey("error"))
        {
            return new OAuthGetTokenRes
            {
                State = LoginState.Error
            };
        }

        var obj2 = obj1.ToObject<OAuthGetCodeObj>();
        if (obj2 == null)
        {
            return new OAuthGetTokenRes
            {
                State = LoginState.DataError
            };
        }

        return new OAuthGetTokenRes
        {
            State = LoginState.Done,
            Obj = obj2
        };
    }

    /// <summary>
    /// 取消请求
    /// </summary>
    public static void Cancel()
    {
        if (s_cancel.IsCancellationRequested == false)
        {
            s_cancel.Cancel();
        }
    }
}
