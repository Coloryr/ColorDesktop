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
