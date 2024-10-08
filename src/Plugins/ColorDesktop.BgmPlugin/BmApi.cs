using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ColorDesktop.CoreLib;

namespace ColorDesktop.BmPlugin;

public static class BmApi
{
    

    public static async void GetBm()
    {
        var data = await HttpUtils.Client.GetStringAsync("https://api.bgm.tv/calendar");
    }
}
