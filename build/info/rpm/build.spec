name:       colordesktop
Version:    %version%
Release:    1
Summary:    A Minecraft Launcher
License:    Apache-2.0

%description
A Desktop Tools

%pre
rm -f /usr/local/bin/ColorDesktop.Launcher

%post
chmod a+x /usr/share/colordesktop/ColorDesktop.Launcher
chmod a+x /usr/share/applications/ColorDesktop.desktop
sudo ln -s /usr/share/colordesktop/ColorDesktop.Launcher /usr/local/bin

%postup
sudo update-desktop-database /usr/share/applications

%files
/usr/share/applications/ColorDesktop.desktop
/usr/share/colordesktop/
/usr/share/icons/colordesktop.png
