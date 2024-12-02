using System.Security.Cryptography;
using System.Text;
using ColorDesktop.Api;
using Newtonsoft.Json;

namespace ColorDesktop.PluginList;

public class Program
{
    public const string ConfigName = "plugin.json";

    /// <summary>
    /// 获取所有文件
    /// </summary>
    /// <param name="local">路径</param>
    /// <returns>文件列表</returns>
    public static List<FileInfo> GetAllFile(string local)
    {
        var list = new List<FileInfo>();
        var info = new DirectoryInfo(local);
        if (!info.Exists)
        {
            return list;
        }

        list.AddRange(info.GetFiles());
        foreach (var item in info.GetDirectories())
        {
            list.AddRange(GetAllFile(item.FullName));
        }

        return list;
    }

    /// <summary>
    /// 获取SHA1值
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    public static string GenSha1(Stream stream)
    {
        var text = new StringBuilder();
        foreach (byte item in SHA1.HashData(stream))
        {
            text.AppendFormat("{0:x2}", item);
        }
        return text.ToString().ToLower();
    }

    static void Main(string[] args)
    {
        Directory.CreateDirectory("web");
        if (!Directory.Exists("plugins"))
        {
            Console.WriteLine("没有找到组件目录");
        }
        var plugins = Directory.GetDirectories("plugins");

        var download = new PluginDownloadObj()
        {
            Source = "Coloryr",
            BaseUrl = "https://www.coloryr.com/colordesktop",
            Plugins = []
        };

        var list = new HashSet<string>();

        foreach (var item in plugins)
        {
            try
            {
                var list2 = GetAllFile(item);
                var config = list2.FirstOrDefault(item =>
                    item.Name.Equals(ConfigName, StringComparison.CurrentCultureIgnoreCase));
                if (config == null)
                {
                    continue;
                }
                var obj = JsonConvert.DeserializeObject<PluginDataObj>(File.ReadAllText(config.FullName));
                if (obj == null)
                {
                    continue;
                }

                var ass = new PluginAssembly(config.DirectoryName!, obj);
                ass.FindDll();

                var web = $"web/{obj.ID}";
                Directory.CreateDirectory(web);

                var pluginitem = new PluginDownloadObj.ItemObj()
                {
                    ID = obj.ID,
                    Auther = obj.Auther,
                    Name = obj.Name,
                    Version = obj.Version,
                    Describe = obj.Describe,
                    Deps = [],
                    Files = [],
                    Os = obj.Os,
                    Url = "/" + obj.ID,
                    ApiVersion = obj.ApiVersion
                };

                foreach (var item1 in obj.Dependents)
                {
                    pluginitem.Deps.Add(item1.ID);
                }

                foreach (var item1 in list2)
                {
                    using var stream = File.Open(web + "/" + item1.Name, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                    using var stream1 = File.OpenRead(item1.FullName);
                    stream1.CopyTo(stream);
                    stream1.Seek(0, SeekOrigin.Begin);
                    pluginitem.Files.Add(new()
                    {
                        Name = item1.Name,
                        Sha1 = GenSha1(stream1)
                    });
                }

                var icon = ass.Plugin.GetIcon();
                if (icon != null)
                {
                    using var stream = File.Open(web + "/icon.png", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                    icon.CopyTo(stream);
                    pluginitem.Icon = "icon.png";
                }

                download.Plugins.Add(pluginitem);
            }
            catch (Exception e)
            {
                list.Add(item);

                Console.WriteLine(string.Format("组件 {0} 生成信息失败", item), e);
            }
        }

        var temp = JsonConvert.SerializeObject(download);
        File.WriteAllText("web/plugin.json", temp);
    }
}
