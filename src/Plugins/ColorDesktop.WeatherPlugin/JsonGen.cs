using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ColorDesktop.WeatherPlugin.Objs;

namespace ColorDesktop.WeatherPlugin;

[JsonSerializable(typeof(WeatherInstanceObj))]
[JsonSerializable(typeof(WeatherConfigObj))]
[JsonSerializable(typeof(List<City1Obj>))]
[JsonSerializable(typeof(WeatherInfoObj))]
public partial class JsonGen : JsonSerializerContext
{
}
