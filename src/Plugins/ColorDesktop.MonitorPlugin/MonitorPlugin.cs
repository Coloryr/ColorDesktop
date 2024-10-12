using System.Diagnostics;
using Avalonia.Controls;
using ColorDesktop.Api;
using LibreHardwareMonitor.Hardware;

namespace ColorDesktop.MonitorPlugin;

public class MonitorPlugin : IPlugin
{
    public bool IsCoreLib => false;

    public bool HavePluginSetting => true;

    public bool HaveInstanceSetting => true;

    public InstanceDataObj CreateInstanceDefault()
    {
        throw new NotImplementedException();
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

    public void Init(string local, string local1, LanguageType type)
    {
        Computer computer = new Computer
        {
            IsCpuEnabled = true,
            IsGpuEnabled = true,
            IsMemoryEnabled = true,
            IsMotherboardEnabled = true,
            IsControllerEnabled = true,
            IsNetworkEnabled = true,
            IsStorageEnabled = true
        };

        computer.Open();

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

        computer.Close();
    }

    public IInstance MakeInstances(InstanceDataObj obj)
    {
        throw new NotImplementedException();
    }

    public Control OpenSetting(InstanceDataObj instance)
    {
        throw new NotImplementedException();
    }

    public Control OpenSetting()
    {
        throw new NotImplementedException();
    }

    public void Stop()
    {
        
    }
}
