﻿using System.Collections.Generic;
using ColorDesktop.Api;

namespace ColorDesktop.Launcher.Objs;

public record ConfigObj
{
    public bool AutoStart { get; set; }
    public bool AutoMin { get; set; }
    public WindowTransparencyType Tran { get; set; }
    public List<string> EnablePlugin { get; set; }
    public List<string> EnableInstance { get; set; }
}
