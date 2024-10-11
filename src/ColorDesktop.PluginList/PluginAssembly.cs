using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using ColorDesktop.Api;

namespace ColorDesktop.PluginList;

public class PluginAssembly : AssemblyLoadContext
{
    public PluginDataObj Obj { get; init; }

    public IPlugin Plugin { get; private set; }
    public string Local { get; init; }

    public bool Enable { get; set; }

    public readonly List<PluginAssembly> Deps = [];

    public PluginAssembly(string local, PluginDataObj obj) : base(obj.ID, true)
    {
        Local = local;
        Obj = obj;

        foreach (var item in obj.Dlls)
        {
            LoadDll(local, item);
        }
    }

    protected override Assembly? Load(AssemblyName name)
    {
        foreach (var item in Deps)
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

    public void AddShare(PluginAssembly ass)
    {
        Deps.Add(ass);
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