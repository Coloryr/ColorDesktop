#!/usr/bin/env bash

VER=1
REL=1
URL='https://github.com/Coloryr/ColorDesktop/releases/download/a1.2024.10.11-1/colordesktop-linux-a1-1-x86_64.pkg.tar.zst'
TARGET='colordesktop-linux-a1-1-x86_64.pkg.tar.zst'
POSTURL=$(echo $URL | sed 's/\//\\\//g')
wget $URL -O $TARGET
SUM=$(sha256sum $TARGET | cut -f1 -d' ')

rm -rf out
mkdir out
cp PKGBUILD_Pre out/PKGBUILD
cp colordesktop.install out/colordesktop.install

cd out

sed -i "s/%ver%/$VER/g" PKGBUILD
sed -i "s/%rel%/$REL/g" PKGBUILD
sed -i "s/%source%/$POSTURL/g" PKGBUILD
sed -i "s/%sha%/$SUM/g" PKGBUILD
sed -i "s/%file%/$TARGET/g" PKGBUILD

makepkg --printsrcinfo > .SRCINFO
cd ../
