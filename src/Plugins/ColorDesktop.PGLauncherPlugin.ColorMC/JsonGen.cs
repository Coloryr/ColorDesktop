using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ColorDesktop.PGLauncherPlugin.ColorMC;

[JsonSerializable(typeof(PGColorMCInstanceObj))]
[JsonSerializable(typeof(PGColorMCConfigObj))]
[JsonSerializable(typeof(GameSettingObj))]
public partial class JsonGen1 : JsonSerializerContext
{
}
