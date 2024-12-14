#!/bin/bash

mkdir ./build_out

dotnet restore ./src/ColorDesktop.sln

cd ./src/Core/ColorDesktop.Web

dotnet msbuild -p:RuntimeIdentifier=win-x64
dotnet msbuild -p:RuntimeIdentifier=win-arm64
dotnet msbuild -p:RuntimeIdentifier=linux-x64
dotnet msbuild -p:RuntimeIdentifier=linux-arm64
dotnet msbuild -p:RuntimeIdentifier=osx-x64
dotnet msbuild -p:RuntimeIdentifier=osx-arm64

cd ./src/build_out/Debug/net8.0/

zip -r webplugin-linux-arm64.zip WebPlugin/Debug/net8.0/linux-arm64
zip -r webplugin-linux-x64.zip WebPlugin/Debug/net8.0/linux-x64
zip -r webplugin-osx-arm64.zip WebPlugin/Debug/net8.0/osx-arm64
zip -r webplugin-osx-x64.zip WebPlugin/Debug/net8.0/osx-x64
zip -r webplugin-win-arm64.zip WebPlugin/Debug/net8.0/win-arm64
zip -r webplugin-win-x64.zip WebPlugin/Debug/net8.0/win-x64