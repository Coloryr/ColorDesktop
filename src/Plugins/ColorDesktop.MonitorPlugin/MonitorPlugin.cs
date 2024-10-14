using System.Diagnostics;
using Avalonia.Controls;
using ColorDesktop.Api;
using ColorDesktop.CoreLib;
using LibreHardwareMonitor.Hardware;

namespace ColorDesktop.MonitorPlugin;

public class MonitorPlugin : IPlugin
{
    public const string ConfigName = "monitor.json";

    public static MonitorInstanceObj GetConfig(InstanceDataObj obj)
    {
        return InstanceUtils.GetConfig(obj, new MonitorInstanceObj()
        {
            
        }, ConfigName);
    }

    public static void SaveConfig(InstanceDataObj obj, MonitorInstanceObj config)
    {
        InstanceUtils.SaveConfig(obj, config, ConfigName);
    }

    public bool IsCoreLib => false;

    public bool HavePluginSetting => false;

    public bool HaveInstanceSetting => true;

    private Computer computer;

    public InstanceDataObj CreateInstanceDefault()
    {
        return new InstanceDataObj()
        {
            Nick = "硬件监控",
            Plugin = "coloryr.monitor",
            Pos = PosEnum.TopRight,
            Margin = new(5)
        };
    }

    public void Disable()
    {
        
    }

    public void Enable()
    {
        
    }

    public Stream? GetIcon()
    {
        return null;
    }

    public class UpdateVisitor : IVisitor
    {
        public void VisitComputer(IComputer computer)
        {
            computer.Traverse(this);
        }
        public void VisitHardware(IHardware hardware)
        {
            hardware.Update();
            foreach (IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
        }
        public void VisitSensor(ISensor sensor) { }
        public void VisitParameter(IParameter parameter) { }
    }

    public void Init(string local, string local1)
    {
        computer = new Computer
        {
            IsCpuEnabled = true,
            IsGpuEnabled = true,
            IsMemoryEnabled = true,
            IsMotherboardEnabled = true,
            IsControllerEnabled = true,
            IsNetworkEnabled = true,
            IsStorageEnabled = true,
            IsBatteryEnabled = true,
            IsPsuEnabled = true
        };

        Task.Run(() =>
        {
            computer.Open();
            computer.Accept(new UpdateVisitor());

            foreach (IHardware hardware in computer.Hardware)
            {
                Console.WriteLine("Hardware: {0}", hardware.Name);

                foreach (IHardware subhardware in hardware.SubHardware)
                {
                    Console.WriteLine("\tSubhardware: {0}", subhardware.Name);

                    foreach (ISensor sensor in subhardware.Sensors)
                    {
                        Console.WriteLine("\t\tSensor: {0}, value: {1}", sensor.Name, sensor.Value);
                    }
                }

                foreach (ISensor sensor in hardware.Sensors)
                {
                    Console.WriteLine("\tSensor: {0}, value: {1}", sensor.Name, sensor.Value);
                }
            }
        });
    }

    public IInstance MakeInstances(InstanceDataObj obj)
    {
        throw new NotImplementedException();
    }

    public Control OpenSetting(InstanceDataObj instance)
    {
        return new MonitorInstanceSettingControl(instance);
    }

    public Control OpenSetting()
    {
        throw new NotImplementedException();
    }

    public void Stop()
    {
        computer.Close();
    }

    public void LoadLang(LanguageType type)
    {
        
    }
}
