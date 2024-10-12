using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ColorDesktop.Launcher.Hook;

public class Linux
{
    public static void SetLaunch(bool start)
    {
        Task.Run(() =>
        {
            try
            {
                // if (!File.Exists("/etc/systemd/user/colordesktop.service"))
                // {
                //     {
                //         var assm = Assembly.GetExecutingAssembly();
                //         using var item = assm.GetManifestResourceStream("ColorDesktop.Launcher.Resource.linux.service")!;
                //         using var file = File.Open(Program.RunDir + "colordesktop.service", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                //         item.CopyTo(file);
                //     }
                //     Run("pkexec","cp " + Program.RunDir + "colordesktop.service" + " /etc/systemd/user/colordesktop.service");
                //     Run("systemctl", "--user daemon-reload");
                // }

                // if (start)
                // {
                //     Run("systemctl", "--user enable colordesktop.service");
                // }
                // else
                // {
                //     Run("systemctl", "--user disable colordesktop.service");
                // }

                if (start)
                {
                    if (!File.Exists("/etc/xdg/autostart/colordesktop.desktop"))
                    {
                        var assm = Assembly.GetExecutingAssembly();
                        using var item = assm.GetManifestResourceStream("ColorDesktop.Launcher.Resource.linux.desktop")!;
                        using var file = File.Open(Program.RunDir + "colordesktop.desktop", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                        item.CopyTo(file);
                    }

                    Run("pkexec","cp " + Program.RunDir + "colordesktop.desktop" + " /etc/xdg/autostart/colordesktop.desktop");
                }
                else
                {
                    if (File.Exists("/etc/xdg/autostart/colordesktop.desktop"))
                    {
                        Run("pkexec","rm /etc/xdg/autostart/colordesktop.desktop");
                    }
                }
            }
            catch (Exception e)
            {

            }
        });
    }

    private static void Run(string pg, string cmd)
    {
        var p = Process.Start(pg, cmd);
        p.WaitForExit();
    }
}
