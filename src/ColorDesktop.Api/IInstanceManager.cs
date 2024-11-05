using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorDesktop.Api;

public interface IInstanceManager
{
    IReadOnlyList<string> GetInstances();
    InstanceDataObj? GetInstanceData(string key);
    InstanceState? GetState(string key);
}
