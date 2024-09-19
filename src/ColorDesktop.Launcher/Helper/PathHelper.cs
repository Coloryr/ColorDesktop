using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorDesktop.Launcher.Helper;

public static class PathHelper
{
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
}
