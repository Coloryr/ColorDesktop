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