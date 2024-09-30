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

build_arch() 
{
    pkg=colordesktop-linux-$main_version$version-1-$2.pkg.tar.zst

    echo "build $pkg"

    base=./src/build_out/$1-dotnet
    base_dir="$base/colordesktop_arch"

    mkdir $base_dir

    pdbs=("ColorDesktop.Api.pdb" "ColorDesktop.Launcher" "ColorDesktop.Launcher.pdb" 
        "libHarfBuzzSharp.so" "libSkiaSharp.so")

    for line in ${pdbs[@]}
    do
        cp $base/$line \
            $base_dir/$line
    done

    info=./build/info/linux/usr/share

    cp ./build/info/arch/PKGBUILD $base_dir/PKGBUILD
    cp ./build/info/arch/colordesktop.install $base_dir/colordesktop.install
    cp $info/applications/ColorDesktop.desktop $base_dir/ColorDesktop.desktop
    cp $info/icons/colordesktop.png $base_dir/colordesktop.png

    sed -i "s/%version%/$version/g" $base_dir/PKGBUILD
    sed -i "s/%arch%/$2/g" $base_dir/PKGBUILD

    cd $base_dir
    makepkg -f

    cd ../../../../

    cp $base_dir/colordesktop-$version-1-$2.pkg.tar.zst ./build_out/$pkg

    echo "$pkg build done"
}

build_arch linux-x64 x86_64
# build_arch linux-arm64 aarch64
