#!/bin/bash

mkdir ./build_out

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