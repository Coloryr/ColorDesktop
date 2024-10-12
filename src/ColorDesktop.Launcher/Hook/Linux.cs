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
                if (!File.Exists("/etc/systemd/user/colordesktop.service"))
                {
                    {
                        var assm = Assembly.GetExecutingAssembly();
                        using var item = assm.GetManifestResourceStream("ColorDesktop.Launcher.Resource.linux.service")!;
                        using var file = File.Open(Program.RunDir + "/colordesktop.service", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                        item.CopyTo(file);
                    }
                    Run("cp " + Program.RunDir + "/colordesktop.service" + " /etc/systemd/user/colordesktop.service");
                    Run("systemctl daemon-reloade");
                }

                if (start)
                {
                    Run("systemctl enable colordesktop.service");
                }
                else
                {
                    Run("systemctl disable colordesktop.service");
                }
            }
            catch (Exception e)
            {

            }
        });
    }

    private static void Run(string cmd)
    {
        var p = Process.Start("sudo", cmd);
        p.WaitForExit();
    }
}
