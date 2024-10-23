using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorDesktop.ToDoPlugin.Objs;
using Newtonsoft.Json;

namespace ColorDesktop.ToDoPlugin.Net;

public static class ToDoApi
{
    public static async Task<ToDoListObj?> GetLists(string bear)
    {
        var req = new HttpRequestMessage()
        {
            RequestUri = new Uri("https://graph.microsoft.com/v1.0/me/todo/lists")
        };
        req.Headers.Add("Authorization", "Bearer " + bear);
        try
        {
            var res = await OAuth.Client.SendAsync(req);
            if (!res.IsSuccessStatusCode)
            {
                return null;
            }
            var data = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ToDoListObj>(data);
        }
        catch
        {
            return null;
        }
    }
}
