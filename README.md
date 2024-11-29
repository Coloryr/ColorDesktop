# ColorDesktop

一个桌面小组件软件

只支持Windows、Linux和macos  
[组件列表](./plugin.md)

![](/pic/pic1.png)

## 如何使用

下载软件后，打开软件  
在左侧点击`组件管理`  
然后点击`新建实例`即可开始使用

主界面点击`打开组件路径`即可打开组件文件夹，复制需要的使用的组件到文件夹中即可  
组件目录一般是这样的结构
```
- ColorDesktop.Launcher.exe                   软件本体
- config.json                                 软件配置文件
- logs.log                                    运行日志
- lock                                        互斥锁
...
- plugins \                                   这个是组件文件夹
    - AnalogClockPlugin \                     这是一个组件，里面应该是这样的文件
        - ColorDesktop.AnalogClockPlugin.dll  组件二进制
        - plugin.json                         组件信息
    - ClockPlugin \                           这是一个组件，里面应该是这样的文件
        - ColorDesktop.ClockPlugin.dll        组件二进制
        - plugin.json                         组件信息
        - clock.json                          组件的一个配置文件
    ...
- instances \                                 这个是实例配置文件夹
    - 66f03335405f4f5a88f31f62dcdb0b09 \      这是一个实例
        - analogclock.json                    组件的实例配置文件
        - instance.json                       实例信息
    - f19343f5c0b9448e963f3124204e8fc5 \      这是一个实例
        - clock.json                          组件的实例配置文件
        - instance.json                       实例信息
    ...
```

## 组件管理

在主界面左侧点击`组件管理`，可以打开组件管理界面  
![](/pic/pic2.png)  

右侧上方是加载信息，组建搜索  
下方是组件列表，你可以对组件进行`启用`与`关闭`，同时也能`新建显示实例`  
某些组件有自己的总体设置  
![](/pic/pic6.png)  

## 实例管理

在主界面左侧点击`实例管理`，可以打开实例管理界面  
![](/pic/pic3.png)  

右侧上方是加载信息，实例搜索  
下方是组件列表，你可以对组件进行`启用`与`关闭`，同时也能`修改实例`  
![](/pic/pic4.png)  
某些组件也有自己的设置选项  
![](/pic/pic5.png)  

## 我要开发组件

前往组件[开发指南](./dev.md)

## 项目说明
- ColorDesktop.Api 插件接口
- ColorDesktop.Debug 调试用
- ColorDesktop.Launcher 程序本体
- ColorDesktop.PluginList 插件源生成器

## 依赖/引用的项目
[AvaloniaUI](https://github.com/AvaloniaUI/Avalonia) 跨平台UI框架  
[DialogHost.Avalonia](https://github.com/AvaloniaUtils/DialogHost.Avalonia) 弹窗库  
[CommunityToolkit.Mvvm](https://github.com/CommunityToolkit/dotnet) MVVM工具  
[Svg.Skia](https://github.com/wieslawsoltes/Svg.Skia) Svg图像显示  
[SkiaSharp](https://github.com/mono/SkiaSharp) Skia图像库  
[DotNetty](https://github.com/Azure/DotNetty) 异步通信框架  
[Newtonsoft.Json](https://www.newtonsoft.com/json) JSON解析器

## 组件依赖/引用的项目
[LibreHardwareMonitor](https://github.com/LibreHardwareMonitor/LibreHardwareMonitor) 传感器读取  
[lunar-csharp](https://github.com/6tail/lunar-csharp) 农历节气天干地支等计算  
[Live2DCSharpSDK](https://github.com/Coloryr/Live2DCSharpSDK) Live2D显示  
[MinecraftSkinRender](https://github.com/Coloryr/MinecraftSkinRender) Minecraft皮肤渲染器

## 开源协议
Apache 2.0  

```
Copyright 2024 coloryr

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
```

附属的开源协议  
MIT  
BSD  
MPL2.0(插件使用该协议)

## 使用的IDE开发工具
[Visual Studio 2022](https://visualstudio.microsoft.com/)  