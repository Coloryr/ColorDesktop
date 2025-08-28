using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ColorDesktop.PGLauncherPlugin;

[JsonSerializable(typeof(PGLauncherInstanceObj))]
[JsonSerializable(typeof(PGLauncherConfigObj))]
public partial class JsonGen : JsonSerializerContext
{
}
