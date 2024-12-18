﻿using System.Reflection;
using ColorDesktop.Api;
using ColorDesktop.Api.Events;
using ColorDesktop.Api.Objs;
using Xilium.CefGlue;
using Xilium.CefGlue.Common;

namespace ColorDesktop.Web;

public class WebDesktop : IPlugin
{
    private string cachePath;

    internal static string InstanceLocal;

    public const string ApiVersion = LauncherApi.ApiVersion;

    internal static IPluginManager Manager;

    public bool CanEnable => false;
    public bool CanCreateInstance => true;
    public bool HavePluginSetting => true;
    public bool HaveInstanceSetting => true;

    public InstanceDataObj CreateInstanceDefault()
    {
        return new InstanceDataObj()
        {
            Nick = "Web",
            Plugin = "coloryr.web",
            Pos = PosEnum.TopRight,
            Margin = new(5)
        };
    }

    public void Disable()
    {

    }

    public void Enable()
    {
        Manager = LauncherApi.Hook.GetPluginManager(this)!;
    }

    public Stream? GetIcon()
    {
        var assm = Assembly.GetExecutingAssembly();
        var item = assm.GetManifestResourceStream("ColorDesktop.Web.icon.png")!;
        return item;
    }

    private static List<DirectoryInfo> FindDirectories(DirectoryInfo info, string name)
    {
        var list = new List<DirectoryInfo>();
        foreach (var dir in info.GetDirectories())
        {
            list.AddRange(FindDirectories(dir, name));
            if (dir.Name == name)
            {
                list.Add(dir);
            }
        }
        return list;
    }

    public void Init(string local, string instance)
    {
        InstanceLocal = instance;
        cachePath = Path.Combine(local, "CefGlue");
        var settings = new CefSettings()
        {
            RootCachePath = cachePath,
            BrowserSubprocessPath = local,
            WindowlessRenderingEnabled = false
        };
        if (SystemInfo.Os != OsType.MacOS)
        {
            foreach (var item in FindDirectories(new DirectoryInfo(local), "locales"))
            {
                if (item.GetFiles().Length > 0)
                {
                    settings.LocalesDirPath = item.FullName;
                    break;
                }
            }
        }
        else 
        {
            foreach (var item in FindDirectories(new DirectoryInfo(local), "Resources"))
            {
                if (item.GetFiles().Length > 0)
                {
                    settings.ResourcesDirPath = item.FullName;
                    break;
                }
            }
        }
        CefRuntimeLoader.Initialize(settings);
        PluginManager.Init(local);
        HttpWeb.Start();
        LauncherApi.AddListener(this, Event);
    }

    private void Event(BaseEvent baseEvent)
    {
        if (baseEvent is InstanceDeleteEvent delete)
        {
            InstanceManager.Remove(delete.UUID);
        }
        else if (baseEvent is PluginReloadEvent)
        {
            PluginManager.Reload();
        }
    }

    public void LoadLang(LanguageType type)
    {

    }

    public IInstance MakeInstances(InstanceDataObj obj)
    {
        var instance = new CefBrowserInstance(obj);
        InstanceManager.Init(obj, instance);
        return instance;
    }

    public InstanceSetting OpenSetting()
    {
        return new() { Control = new SettingControl() };
    }

    public bool Permissions(string key, string permission)
    {
        return false;
    }

    public void Stop()
    {
        HttpWeb.Stop();
        CefRuntime.Shutdown();
    }

    public InstanceSetting OpenSetting(InstanceDataObj instance, bool isNew)
    {
        if (isNew)
        {
            return new()
            {
                Control = new SelectPluginControl(instance)
            };
        }
        else
        {
            if (InstanceManager.InstanceCefs.TryGetValue(instance.UUID, out var view))
            {
                view.OpenSetting();
                return new() { Close = view.CloseSetting };
            }
            else
            {
                return new() 
                { 
                    Control = new CefInstanceSetting(instance).CreateView() 
                };
            }
        }
    }
}
