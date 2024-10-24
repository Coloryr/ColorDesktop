using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorDesktop.ToDoPlugin.Objs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ColorDesktop.ToDoPlugin.Net;

public static class ToDoApi
{
    public static async Task<(bool, ToDoListObj?)> GetLists(string bear)
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
                return (false, null);
            }
            var data = await res.Content.ReadAsStringAsync();

            var obj = JObject.Parse(data);
            if (obj.ContainsKey("error"))
            {
                return (false, null);
            }

            return (true, obj.ToObject<ToDoListObj>());
        }
        catch
        {
            return (false, null);
        }
    }

    public static async Task<(bool, ToDoTaskListObj?)> GetTaskLists(string bear, string id)
    {
        var req = new HttpRequestMessage()
        {
            RequestUri = new Uri($"https://graph.microsoft.com/v1.0/me/todo/lists/{id}/tasks")
        };
        req.Headers.Add("Authorization", "Bearer " + bear);
        try
        {
            var res = await OAuth.Client.SendAsync(req);
            if (!res.IsSuccessStatusCode)
            {
                return (false, null);
            }
            var data = await res.Content.ReadAsStringAsync();

            var obj = JObject.Parse(data);
            if (obj.ContainsKey("error"))
            {
                return (false, null);
            }

            return (true, obj.ToObject<ToDoTaskListObj>());
        }
        catch(Exception e)
        {
            return (false, null);
        }
    }
}
