#!/bin/bash

chmod a+x ./build/update.sh
./build/update.sh

version=""
main_version=""

for line in `cat ./build/version`
do
    version=$line
done

for line in `cat ./build/main_version`
do
    main_version=$line
done

mkdir ./build_out

build_linux() 
{
    echo "build colordesktop-$main_version$version-$1-linux"

    dotnet publish ./src/ColorDesktop.Launcher -p:PublishProfile=$1

    echo "colordesktop-$main_version$version-$1-linux build done"
}

build_linux linux-x64
build_linux linux-arm64