using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using ColorDesktop.Api;
using ColorDesktop.Launcher.Helper;

namespace ColorDesktop.Launcher.Manager;

public class PluginAssembly : AssemblyLoadContext
{
    public PluginDataObj Obj { get; init; }

    public IPlugin Plugin { get; private set; }
    public string Local { get; init; }

    public bool Enable { get; set; }

    private readonly List<PluginAssembly> _deps = [];

    private readonly Dictionary<string, string> _natives = [];

    public PluginAssembly(string local, PluginDataObj obj) : base(obj.ID, true)
    {
        Local = local;
        Obj = obj;

        var dir1 = local + "/runtimes";
        var list = new List<FileInfo>();
        void ReadList(string dir)
        {
            if (Directory.Exists(dir))
            {
                var list1 = PathHelper.GetAllFile(dir);
                foreach (var item in list1)
                {
                    if (item.DirectoryName!.Contains("native"))
                    {
                        _natives.Add(item.Name, item.FullName);
                        continue;
                    }
                    list.Add(item);
                }
                
            }
        }
        if (Directory.Exists(dir1))
        {
            switch (SystemInfo.Os)
            {
                case OsType.Windows:
                    var dir2 = dir1 + "/win";
                    ReadList(dir2);
                    if (SystemInfo.IsArm)
                    {
                        dir2 = dir1 + "/win-arm64";
                        ReadList(dir2);
                    }
                    else
                    {
                        dir2 = dir1 + "/win-x64";
                        ReadList(dir2);
                    }
                    break;
            }
        }

        foreach (var item in obj.Dlls)
        {
            if (list.FirstOrDefault(item1 => item1.Name.Replace(".dll", "") == item) is { } item2)
            {
                LoadDll(item2.DirectoryName!, item2.Name.Replace(".dll", ""));
            }
            else
            {
                LoadDll(local, item);
            }
        }
    }

    protected override Assembly? Load(AssemblyName name)
    {
        foreach (var item in _deps)
        {
            var ass = item.Assemblies
                .Where(a => a.GetName().Name == name.Name)
                .FirstOrDefault();

            if (ass != null)
            {
                return ass;
            }
        }

        return null;
    }

    protected new IntPtr LoadUnmanagedDllFromPath(string unmanagedDllPath)
    {
        return base.LoadUnmanagedDllFromPath(unmanagedDllPath);
    }

    protected override nint LoadUnmanagedDll(string unmanagedDllName)
    {
        if (_natives.TryGetValue(unmanagedDllName, out var dir))
        { 
            
        }
        return base.LoadUnmanagedDll(unmanagedDllName);
    }

    public void AddShare(PluginAssembly ass)
    {
        _deps.Add(ass);
    }

    public void AddLoad(PluginAssembly ass)
    {
        foreach (var item in ass.Obj.Dlls)
        {
            LoadDll(ass.Local, item);
        }
    }

    private void LoadDll(string local, string item)
    {
        var local1 = Path.GetFullPath(local + "/" + item + ".dll");
        var local2 = Path.GetFullPath(local + "/" + item + ".pdb");
        if (!File.Exists(local1))
        {
            return;
        }

        using var stream = File.OpenRead(local1);
        if (File.Exists(local2))
        {
            using var stream1 = File.OpenRead(local2);

            LoadFromStream(stream, stream1);
        }
        else
        {
            LoadFromStream(stream);
        }
    }

    public void FindDll()
    {
        foreach (var item in Assemblies)
        {
            foreach (var item1 in item.GetTypes())
            {
                foreach (var item2 in item1.GetInterfaces())
                {
                    if (item2 == typeof(IPlugin))
                    {
                        Plugin = (Activator.CreateInstance(item1) as IPlugin)!;
                        return;
                    }
                }
            }
        }

        throw new Exception("Plugin validation error");
    }
}