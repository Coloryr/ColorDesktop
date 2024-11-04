#!/bin/bash

mkdir ./build_out

dotnet restore ./src/ColorDesktop.sln

build() 
{
    echo "build colordesktop-plugin-$2"

    dotnet build ./src/Plugins/$1 --configuration Release

    echo "colordesktop-plugin-$2 build done"
}

build ColorDesktop.AnalogClockPlugin AnalogClockPlugin
build ColorDesktop.CalendarPlugin CalendarPlugin
build ColorDesktop.ClockPlugin ClockPlugin
build ColorDesktop.CoreLib CoreLib
build ColorDesktop.WeatherPlugin WeatherPlugin
build ColorDesktop.PGLauncherPlugin PGLauncherPlugin
build ColorDesktop.PGLauncherPlugin.ColorMC PGColorMCPlugin
build ColorDesktop.OneWordPlugin OneWordPlugin
build ColorDesktop.BmPlugin BmPlugin
build ColorDesktop.MonitorPlugin MonitorPlugin
build ColorDesktop.Live2DPlugin Live2DPlugin
build ColorDesktop.MinecraftSkinPlugin MinecraftSkinPlugin
build ColorDesktop.MinecraftMotdPlugin MinecraftMotdPlugin
build ColorDesktop.ToDoPlugin ToDoPlugin
build ColorDesktop.MusicControlPlugin MusicControlPlugin

cd ./src/build_out/Debug/net8.0/plugins/

zip -r ../AnalogClockPlugin.zip AnalogClockPlugin
zip -r ../CalendarPlugin.zip CalendarPlugin
zip -r ../WeatherPlugin.zip WeatherPlugin
zip -r ../ClockPlugin.zip ClockPlugin
zip -r ../CoreLib.zip CoreLib
zip -r ../PGLauncherPlugin.zip PGLauncherPlugin
zip -r ../PGColorMCPlugin.zip PGColorMCPlugin
zip -r ../OneWordPlugin.zip OneWordPlugin
zip -r ../BmPlugin.zip BmPlugin
zip -r ../MonitorPlugin.zip MonitorPlugin
zip -r ../Live2DPlugin.zip Live2DPlugin
zip -r ../MinecraftSkinPlugin.zip MinecraftSkinPlugin
zip -r ../MinecraftMotdPlugin.zip MinecraftMotdPlugin
zip -r ../ToDoPlugin.zip ToDoPlugin
zip -r ../MusicControlPlugin.zip MusicControlPlugin