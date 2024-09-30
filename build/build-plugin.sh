#!/bin/bash

mkdir ./build_out

build() 
{
    echo "build colordesktop-plugin-$1"

    dotnet publish ./src/Plugins/$1 -p:PublishProfile=FolderProfile

    echo "colordesktop-plugin-$1 build done"
}

build ColorDesktop.AnalogClockPlugin
build ColorDesktop.CalendarPlugin
build ColorDesktop.ClockPlugin
build ColorDesktop.CoreLib
build ColorDesktop.WeatherPlugin