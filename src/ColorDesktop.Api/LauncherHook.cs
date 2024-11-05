using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorDesktop.Api;

public static class LauncherHook
{
    public const string ApiVersion = "3";

    public static IInstanceManager InstanceManager { get; private set; }

    public static void Init(IInstanceManager manager)
    {
        InstanceManager ??= manager;
    }
}
