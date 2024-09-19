using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using ColorDesktop.Api;

namespace ColorDesktop.Launcher.Manager;

public class PluginAssembly : AssemblyLoadContext
{
    private readonly PluginDataObj _obj;

    public IPlugin Plugin { get; init; }

    public string Local { get; init; }

    public PluginAssembly(string local, PluginDataObj obj) : base(obj.ID, true)
    {
        Local = local;
        _obj = obj;

        foreach (var item in obj.Dlls)
        {
            var local1 = Path.GetFullPath(local + "/" + item + ".dll");
            var local2 = Path.GetFullPath(local + "/" + item + ".pdb");
            if (!File.Exists(local1))
            {
                continue;
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

        foreach (var item in Assemblies)
        {
            if (Plugin != null)
            {
                break;
            }
            foreach (var item1 in item.GetTypes())
            {
                if (Plugin != null)
                {
                    break;
                }
                foreach (var item2 in item1.GetInterfaces())
                {
                    if (item2 == typeof(IPlugin))
                    {
                        Plugin = (Activator.CreateInstance(item1) as IPlugin)!;
                        break;
                    }
                }
            }
        }

        if (Plugin == null)
        {
            throw new Exception("Plugin validation error");
        }
    }

    protected override Assembly? Load(AssemblyName name)
    {
        foreach (var item in _obj.Dependents)
        {
            if (PluginManager.PluginAssemblys.TryGetValue(item, out var ass))
            {
                var list = ass.Assemblies.Where(a => a.GetName().FullName == name.FullName);
                return list.First();
            }
        }

        return null;
    }
}