#!/bin/bash

sudo apt-get install rpm -y

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

build_rpm()
{
    rpm=colordesktop-linux-$main_version$version-1.$2.rpm

    echo "build $rpm"

    base=./src/build_out/$1-dotnet
    base_dir="$base/colordesktop_rpm"

    mkdir -p $base_dir/{BUILD,RPMS,SOURCES,SPECS,SRPMS,BUILDROOT}

    pdbs=("ColorDesktop.Api.pdb" "ColorDesktop.Launcher" "ColorDesktop.Launcher.pdb" 
        "libHarfBuzzSharp.so" "libSkiaSharp.so")

    bindir=$base_dir/BUILDROOT/colordesktop-$version-1.$2/usr/share

    mkdir -p $bindir
    mkdir -p $bindir/colordesktop
    mkdir -p $bindir/applications
    mkdir -p $bindir/icons

    info=./build/info/linux/usr/share

    cp ./build/info/rpm/build.spec $base_dir/SPECS
    cp $info/applications/ColorDesktop.desktop $bindir/applications/ColorDesktop.desktop
    cp $info/icons/colordesktop.png $bindir/icons/colordesktop.png

    sed -i "s/%version%/$version/g" $base_dir/SPECS/build.spec

    for line in ${pdbs[@]}
    do
        cp $base/$line \
            $bindir/colordesktop/$line
    done

    rpmbuild -bb --target=$2 $base_dir/SPECS/build.spec --define "_topdir %{getenv:PWD}/src/build_out/$1-dotnet/colordesktop_rpm"

    cp $base_dir/RPMS/$2/colordesktop-$version-1.$2.rpm ./build_out/$rpm

    echo "build $rpm done"
}

build_rpm linux-x64 x86_64
build_rpm linux-arm64 aarch64