#!/bin/bash

mkdir ./build_out

build() 
{
    echo "build colordesktop-plugin-$1"

    dotnet build ./src/Plugins/$1 --configuration Release

    echo "colordesktop-plugin-$1 build done"
}

build ColorDesktop.AnalogClockPlugin
build ColorDesktop.CalendarPlugin
build ColorDesktop.ClockPlugin
build ColorDesktop.CoreLib
build ColorDesktop.WeatherPlugin