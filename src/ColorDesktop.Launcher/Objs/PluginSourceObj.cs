﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorDesktop.Launcher.Objs;

public record PluginSourceObj
{
    public string Url { get; set; }
    public bool Enable { get; set; }
}