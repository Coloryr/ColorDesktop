﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorDesktop.Launcher.Helper;

public static class LangHelper
{
    public static string[] GetPluginTypeLang()
    {
        return ["ID", "名字", "作者", "描述"];
    }

    public static string[] GetInstanceTypeLang()
    {
        return ["插件", "名字", "UUID"];
    }
}
