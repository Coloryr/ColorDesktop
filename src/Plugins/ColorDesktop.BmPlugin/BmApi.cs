using System.Text.Json;
using ColorDesktop.CoreLib;

namespace ColorDesktop.BmPlugin;

public static class BmApi
{
    public static async Task<List<BmObj>?> GetBm()
    {
        try
        {
            var data = await HttpUtils.Client.GetStringAsync("https://api.bgm.tv/calendar");

            return JsonSerializer.Deserialize(data, JsonGen.Default.ListBmObj);
        }
        catch(Exception e)
        {

        }

        return null;
    }
}
