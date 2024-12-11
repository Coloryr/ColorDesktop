using System.IO.Compression;
using System.Reflection;
using Avalonia.Controls;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;
using ColorDesktop.CoreLib;
using Live2DCSharpSDK.App;
using Live2DCSharpSDK.Framework;

namespace ColorDesktop.Live2DPlugin;

public class Live2DPlugin : IPlugin
{
    public const string ConfigName = "live2d.json";

    public static bool IsCoreLoad { get; set; }

    private static string s_local;

    private static readonly Dictionary<string, OpenGlPageControl> s_views = [];

    public static Live2DInstanceObj GetConfig(InstanceDataObj obj)
    {
        return InstanceUtils.GetConfig(obj, new Live2DInstanceObj()
        {
            Width = 300,
            Height = 300,
            Models = []
        }, ConfigName);
    }

    public static void SaveConfig(InstanceDataObj obj, Live2DInstanceObj config)
    {
        InstanceUtils.SaveConfig(obj, config, ConfigName);
    }

    public static Live2DModelObj MakeNewItem()
    {
        return new()
        {
            Name = LangApi.GetLang("Live2DPlugin.Name2"),
            Scale = 1.0f
        };
    }

    public static void InitCore()
    {
        try
        {
            var cubismAllocator = new LAppAllocator();
            var cubismOption = new CubismOption()
            {
                LogFunction = Console.WriteLine,
                LoggingLevel = LAppDefine.CubismLoggingLevel
            };
            CubismFramework.StartUp(cubismAllocator, cubismOption);
            IsCoreLoad = true;
        }
        catch
        {
            IsCoreLoad = false;
        }
    }

    public static bool SetLive2DCore(string local)
    {
        using var zip = ZipFile.OpenRead(local);
        string file = "";
        string file1 = s_local + "/runtimes/";
        switch (SystemInfo.Os)
        {
            case OsType.Windows:
                file = "Core/dll/windows/" + (SystemInfo.Is64Bit ? "x86_64" : "x86") + "/Live2DCubismCore.dll";
                file1 += (SystemInfo.Is64Bit ? "win-x64" : "win-x86") + "/native/Live2DCubismCore.dll";
                break;
            case OsType.MacOS:
                file = "Core/dll/macos/libLive2DCubismCore.dylib";
                file1 += "osx/native/Live2DCubismCore.dylib";
                break;
            case OsType.Linux:
                file = SystemInfo.IsArm ? "Core/dll/linux/x86_64/libLive2DCubismCore.so"
                    : "Core/dll/experimental/rpi/libLive2DCubismCore.so";
                file1 += (SystemInfo.IsArm ? "linux-arm64" : "linux-x64") + "/native/Live2DCubismCore.so";
                break;
        }

        var info = new FileInfo(file1);
        info.Directory!.Create();
        if (info.Exists)
        {
            info.Delete();
        }

        file1 = Path.GetFullPath(file1);

        foreach (var item in zip.Entries)
        {
            if (item.FullName.Contains(file))
            {
                item.ExtractToFile(file1);
                return true;
            }
        }

        return false;
    }

    public static void AddView(string uuid, OpenGlPageControl view)
    {
        if (!s_views.TryAdd(uuid, view))
        {
            s_views[uuid] = view;
        }
    }

    public static void Update(string uuid)
    {
        if (s_views.TryGetValue(uuid, out var view))
        {
            view.Update();
        }
    }

    public bool CanCreateInstance => true;
    public bool HavePluginSetting => true;
    public bool HaveInstanceSetting => true;
    public bool CanEnable => true;

    public InstanceDataObj CreateInstanceDefault()
    {
        return new InstanceDataObj()
        {
            Nick = LangApi.GetLang("Live2DPlugin.Name1"),
            Plugin = "coloryr.live2d",
            Pos = PosEnum.TopRight,
            Margin = new(5)
        };
    }

    public void Disable()
    {

    }

    public void Enable()
    {

    }

    public Stream? GetIcon()
    {
        var assm = Assembly.GetExecutingAssembly();
        var item = assm.GetManifestResourceStream("ColorDesktop.Live2DPlugin.icon.png")!;
        return item;
    }

    public void Init(string local, string local1)
    {
        s_local = local;

        InitCore();
    }

    public void LoadLang(LanguageType type)
    {
        var assm = Assembly.GetExecutingAssembly();
        if (assm == null)
        {
            return;
        }
        string name = type switch
        {
            LanguageType.en_us => "ColorDesktop.Live2DPlugin.Resource.Lang.en-us.json",
            _ => "ColorDesktop.Live2DPlugin.Resource.Lang.zh-cn.json"
        };
        using var item = assm.GetManifestResourceStream(name)!;
        using var reader = new StreamReader(item);
        LangApi.AddLangs(reader.ReadToEnd());
    }

    public IInstance MakeInstances(InstanceDataObj obj)
    {
        return new Live2DControl();
    }

    public InstanceSetting OpenSetting(InstanceDataObj instance, bool isNew)
    {
        return new() { Control = new Live2DInstanceSettingControl(instance) };
    }

    public InstanceSetting OpenSetting()
    {
        return new() { Control = new Live2DSettingControl() };
    }

    public void Stop()
    {

    }

    public bool Permissions(string key, string permission)
    {
        return false;
    }
}
