#!/bin/bash

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

build_osx()
{    
    zip_name="colordesktop-macos-$main_version$version-$2.zip"

    echo "build $zip_name"

    mkdir ./build_out

    base=./src/build_out/$1-dotnet
    base_dir="$base/ColorDesktop.app/Contents"

    dotnet publish ./src/ColorDesktop.Launcher -p:PublishProfile=$1

    mkdir $base/ColorDesktop.app
    mkdir $base_dir

    files=("ColorDesktop.Api.pdb" "ColorDesktop.Launcher" "ColorDesktop.Launcher.pdb"
        "libAvaloniaNative.dylib" "libHarfBuzzSharp.dylib" "libSkiaSharp.dylib")

    cp -r ./build/info/osx/* $base_dir

    dir=$base_dir/MacOS

    mkdir $dir

    for line in ${files[@]}
    do
        cp $base/$line \
            $dir/$line
    done

    chmod a+x $dir/ColorDesktop.Launcher

    cd ./src/build_out/$1-dotnet
    codesign --force --deep --sign - ColorDesktop.app
    zip -r $zip_name ./ColorDesktop.app
    mv $zip_name ../../../build_out/$zip_name
    cd ../../../

    echo "$zip_name build done"
}

build_osx osx-x64 x86_64
build_osx osx-arm64 aarch64