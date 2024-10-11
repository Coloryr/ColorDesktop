using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ColorDesktop.CoreLib;
using Newtonsoft.Json;

namespace ColorDesktop.BmPlugin;

public static class BmApi
{
    public static async Task<List<BmObj>?> GetBm()
    {
        try
        {
            var data = await HttpUtils.Client.GetStringAsync("https://api.bgm.tv/calendar");

            return JsonConvert.DeserializeObject<List<BmObj>?>(data);
        }
        catch
        { 
            
        }

        return null;
    }
}
