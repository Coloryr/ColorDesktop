using System;

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

    public static string[] GetWindowTranTypeLang()
    {
        return ["无", "透明", "模糊", "亚克力模糊", "云母"];
    }
}
