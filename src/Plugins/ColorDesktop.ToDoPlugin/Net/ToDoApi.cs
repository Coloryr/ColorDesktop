using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ColorDesktop.Api;
using ColorDesktop.ToDoPlugin.Objs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ColorDesktop.ToDoPlugin.Net;

public static class ToDoApi
{
    private static string BuildTime(DateTime time)
    {
        time = new DateTime(time.Year, time.Month, time.Day, 0, 0, 0, time.Kind).ToUniversalTime();
        return $"{time.Year}-{time.Month:00}-{time.Day:00}T{time.Hour:00}:00:00";
    }

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
            Logs.Error("todo error", e);
            return (false, null);
        }
    }

    public static async Task<(bool, ToDoTaskListObj.ValueObj?)> GetTask(string bear, string id, string task)
    {
        var req = new HttpRequestMessage()
        {
            RequestUri = new Uri($"https://graph.microsoft.com/v1.0/me/todo/lists/{id}/tasks/{task}")
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

            return (true, obj.ToObject<ToDoTaskListObj.ValueObj>());
        }
        catch (Exception e)
        {
            Logs.Error("todo error", e);
            return (false, null);
        }
    }

    public static async Task<bool> CreateTask(string bear, string id, 
        string title, string? body = null, DateTime? dueTime = null)
    {
        var obj = new JObject
        {
            { "title", title }
        };
        if (body != null)
        {
            obj.Add("body", new JObject()
            {
                { "content", body },
                { "contentType", "text" }
            });
        }
        if (dueTime is { } time)
        {
            obj.Add("dueDateTime", new JObject()
            {
                { "dateTime", time.ToUniversalTime().ToString() },
                { "timeZone", "UTC" }
            });
        }
        var content = new StringContent(obj.ToString(), Encoding.UTF8, "application/json");
        var req = new HttpRequestMessage()
        {
            RequestUri = new Uri($"https://graph.microsoft.com/v1.0/me/todo/lists/{id}/tasks"),
            Method = HttpMethod.Post,
            Content = content
        };
        req.Headers.Add("Authorization", "Bearer " + bear);
        try
        {
            var res = await OAuth.Client.SendAsync(req);

            return res.StatusCode == HttpStatusCode.Created;
        }
        catch (Exception e)
        {
            Logs.Error("todo error", e);
            return false;
        }
    }

    public static async Task<bool> CreateTaskCheckList(string bear, string id,
       string task, string title)
    {
        var obj = new JObject
        {
            { "displayName", title }
        };
        var content = new StringContent(obj.ToString(), Encoding.UTF8, "application/json");
        var req = new HttpRequestMessage()
        {
            RequestUri = new Uri($"https://graph.microsoft.com/v1.0/me/todo/lists/{id}/tasks/{task}/checklistItems"),
            Method = HttpMethod.Post,
            Content = content
        };
        req.Headers.Add("Authorization", "Bearer " + bear);
        try
        {
            var res = await OAuth.Client.SendAsync(req);

            return res.StatusCode == HttpStatusCode.Created;
        }
        catch (Exception e)
        {
            Logs.Error("todo error", e);
            return false;
        }
    }

    public static async Task<bool> DeleteTaskCheckList(string bear, string id,
      string task, string check)
    {
        var req = new HttpRequestMessage()
        {
            RequestUri = new Uri($"https://graph.microsoft.com/v1.0/me/todo/lists/{id}/tasks/{task}/checklistItems/{check}"),
            Method = HttpMethod.Delete
        };
        req.Headers.Add("Authorization", "Bearer " + bear);
        try
        {
            var res = await OAuth.Client.SendAsync(req);

            return res.StatusCode == HttpStatusCode.NoContent;
        }
        catch (Exception e)
        {
            Logs.Error("todo error", e);
            return false;
        }
    }

    public static async Task<bool> EditCheckItem(string bear, string listId, string task, string id, bool? isCheck, string? text)
    {
        var obj = new JObject();
        if (isCheck is { } check)
        {
            obj.Add("isChecked", check);
        }
        if (text != null)
        {
            obj.Add("displayName", text);
        }
        var content = new StringContent(obj.ToString(), Encoding.UTF8, "application/json");
        var req = new HttpRequestMessage()
        {
            RequestUri = new Uri($"https://graph.microsoft.com/v1.0/me/todo/lists/{listId}/tasks/{task}/checklistItems/{id}"),
            Method = HttpMethod.Patch,
            Content = content
        };
        req.Headers.Add("Authorization", "Bearer " + bear);
        try
        {
            var res = await OAuth.Client.SendAsync(req);

            return res.StatusCode == HttpStatusCode.OK;
        }
        catch (Exception e)
        {
            Logs.Error("todo error", e);
            return false;
        }
    }

    public static async Task<bool> DeleteTask(string bear, string id, string task)
    {
        var req = new HttpRequestMessage()
        {
            RequestUri = new Uri($"https://graph.microsoft.com/v1.0/me/todo/lists/{id}/tasks/{task}"),
            Method = HttpMethod.Delete
        };
        req.Headers.Add("Authorization", "Bearer " + bear);
        try
        {
            var res = await OAuth.Client.SendAsync(req);

            return res.StatusCode == HttpStatusCode.NoContent;
        }
        catch (Exception e)
        {
            Logs.Error("todo error", e);
            return false;
        }
    }

    public static async Task<bool> EditTaskItem(string bear, string listId, string task, string? text, bool? isCheck, DateTime? time, string? body)
    {
        var obj = new JObject();
        if (text != null)
        {
            obj.Add("title", text);
        }
        if (isCheck is { } check)
        {
            if (check)
            {
                obj.Add("completedDateTime", new JObject()
                {
                    { "timeZone", "UTC" },
                    { "dateTime", BuildTime(DateTime.UtcNow) }
                });
            }
            else
            {
                obj.Add("completedDateTime", null);
            }
        }
        if (time is { } time1)
        {
            obj.Add("dueDateTime", new JObject()
            {
                { "timeZone", "UTC" },
                { "dateTime", BuildTime(time1.ToUniversalTime()) }
            });
        }
        if (body != null)
        {
            obj.Add("body", new JObject()
            {
                { "contentType", "text" },
                { "content", body }
            });
        }
        var content = new StringContent(obj.ToString(), Encoding.UTF8, "application/json");
        var req = new HttpRequestMessage()
        {
            RequestUri = new Uri($"https://graph.microsoft.com/v1.0/me/todo/lists/{listId}/tasks/{task}"),
            Method = HttpMethod.Patch,
            Content = content
        };
        req.Headers.Add("Authorization", "Bearer " + bear);
        try
        {
            var res = await OAuth.Client.SendAsync(req);

            return res.StatusCode == HttpStatusCode.OK;
        }
        catch (Exception e)
        {
            Logs.Error("todo error", e);
            return false;
        }
    }
}
