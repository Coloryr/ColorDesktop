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

build_deb() 
{
    deb=colordesktop-linux-$main_version$version-$2.deb

    echo "build $deb"

    base=./src/build_out/$1-dotnet
    base_dir="$base/colordesktop_deb"

    mkdir $base_dir

    pdbs=("ColorDesktop.Api.pdb" "ColorDesktop.Launcher" "ColorDesktop.Launcher.pdb" 
        "libHarfBuzzSharp.so" "libSkiaSharp.so")

    cp -r ./build/info/linux/* $base_dir
    cp -r ./build/info/$1/* $base_dir

    sed -i "s/%version%/$version/g" $base_dir/DEBIAN/control

    dir=usr/share/ColorDesktop

    mkdir $base_dir/$dir

    for line in ${pdbs[@]}
    do
        cp $base/$line \
            $base_dir/$dir/$line
    done

    chmod -R 775 $base_dir/DEBIAN/postinst

    dpkg -b $base_dir ./build_out/$deb

    echo "$deb build done"
}

build_deb linux-x64 x86_64
build_deb linux-arm64 arm64

build_run=./build_run

mkdir $build_run

if [ ! -f "$build_run/deb2appimage.AppImage" ];then
    wget https://github.com/simoniz0r/deb2appimage/releases/download/v0.0.5/deb2appimage-0.0.5-x86_64.AppImage
    mv ./deb2appimage-0.0.5-x86_64.AppImage $build_run/deb2appimage.AppImage
fi

chmod a+x $build_run/deb2appimage.AppImage

sudo apt-get install libfuse2 curl -y

build_appimage()
{
    appimg=colordesktop-linux-$main_version$version-$1.AppImage
    
    build_dir=$build_run/$1
    
    mkdir $build_dir

    echo "build $appimg"

    cp ./build/info/appimg.json $build_dir/appimg.json

    arch=amd64
    deb_name=colordesktop-linux-$main_version$version-$1.deb

    sed -i "s/%version%/$main_version$version/g" $build_dir/appimg.json
    sed -i "s/%arch%/$arch/g" $build_dir/appimg.json
    sed -i "s/%deb_name%/$deb_name/g" $build_dir/appimg.json

    sudo $build_run/deb2appimage.AppImage -j $build_dir/appimg.json -o ./build_out

    sudo chown $USER:$USER ./build_out/colordesktop-$main_version$version-$1.AppImage
    chmod a+x build_out/colordesktop-$main_version$version-$1.AppImage
    mv build_out/colordesktop-$main_version$version-$1.AppImage build_out/$appimg

    echo "$appimg build done"
}

build_appimage x86_64
