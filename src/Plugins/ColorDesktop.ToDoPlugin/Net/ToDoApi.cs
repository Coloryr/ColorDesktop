using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using ColorDesktop.Api;
using ColorDesktop.ToDoPlugin.Objs;

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
            using var data = await res.Content.ReadAsStreamAsync();
            var obj = JsonUtils.ReadAsObj(data);
            if (obj == null || obj.ContainsKey("error"))
            {
                return (false, null);
            }

            return (true, JsonSerializer.Deserialize(obj, JsonGen.Default.ToDoListObj));
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
            using var data = await res.Content.ReadAsStreamAsync();

            var obj = JsonUtils.ReadAsObj(data);
            if (obj == null || obj.ContainsKey("error"))
            {
                return (false, null);
            }

            return (true, JsonSerializer.Deserialize(obj, JsonGen.Default.ToDoTaskListObj));
        }
        catch (Exception e)
        {
            Logs.Error("todo error", e);
            return (false, null);
        }
    }

    public static async Task<(bool, ToDoTaskListObj.ValueObj1?)> GetTask(string bear, string id, string task)
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
            using var data = await res.Content.ReadAsStreamAsync();

            var obj = JsonUtils.ReadAsObj(data);
            if (obj == null || obj.ContainsKey("error"))
            {
                return (false, null);
            }

            return (true, JsonSerializer.Deserialize(obj, JsonGen.Default.ValueObj1));

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
        var obj = new JsonObject
        {
            { "title", title }
        };
        if (body != null)
        {
            obj.Add("body", new JsonObject()
            {
                { "content", body },
                { "contentType", "text" }
            });
        }
        if (dueTime is { } time)
        {
            obj.Add("dueDateTime", new JsonObject()
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
        var obj = new JsonObject
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

    public static async Task<bool> EditCheckItem(string bear, string listId,
        string task, string id, bool? isCheck, string? text)
    {
        var obj = new JsonObject();
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

    public static async Task<bool> EditTaskItem(string bear, string listId, string task,
        string? text, bool? isCheck, DateTime? time, string? body,
        bool? removeTime, DateTime? isReminderTime, bool? isReminder)
    {
        var obj = new JsonObject();
        if (text != null)
        {
            obj.Add("title", text);
        }
        if (isCheck is { } check)
        {
            if (check)
            {
                obj.Add("status", "completed");
            }
            else
            {
                obj.Add("status", "notStarted");
            }
        }
        if (isReminder != null)
        {
            if (isReminder == true && isReminderTime is { } time1)
            {
                obj.Add("isReminderOn", true);
                obj.Add("reminderDateTime", new JsonObject()
                {
                    { "timeZone", "UTC" },
                    { "dateTime", $"{time1.Year}-{time1.Month:00}-{time1.Day:00}T{time1.Hour:00}:00:00" }
                });
            }
            else
            {
                obj.Add("isReminderOn", false);
                obj.Add("reminderDateTime", null);
            }
        }
        if (removeTime == true)
        {
            obj.Add("dueDateTime", null);
        }
        else if (time is { } time1)
        {
            obj.Add("dueDateTime", new JsonObject()
            {
                { "timeZone", "UTC" },
                { "dateTime", $"{time1.Year}-{time1.Month:00}-{time1.Day:00}T{time1.Hour:00}:00:00" }
            });
        }
        if (body != null)
        {
            obj.Add("body", new JsonObject()
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

            if (res.StatusCode != HttpStatusCode.OK)
            {
                var res1 = await res.Content.ReadAsStringAsync();
            }

            return res.StatusCode == HttpStatusCode.OK;
        }
        catch (Exception e)
        {
            Logs.Error("todo error", e);
            return false;
        }
    }

    public static async Task<bool> CreateTaskList(string bear, string name)
    {
        var obj = new JsonObject
        {
            { "displayName", name }
        };
        var content = new StringContent(obj.ToString(), Encoding.UTF8, "application/json");
        var req = new HttpRequestMessage()
        {
            RequestUri = new Uri($"https://graph.microsoft.com/v1.0/me/todo/lists"),
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

    public static async Task<bool> EditTaskList(string bear, string id, string title)
    {
        var obj = new JsonObject
        {
            { "displayName", title }
        };
        var content = new StringContent(obj.ToString(), Encoding.UTF8, "application/json");
        var req = new HttpRequestMessage()
        {
            RequestUri = new Uri($"https://graph.microsoft.com/v1.0/me/todo/lists/{id}"),
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
