﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorDesktop.MonitorPlugin;

public interface IUpdate
{
    void Update(MonitorItemModel model);
    void Reload(MonitorItemModel model);
}
