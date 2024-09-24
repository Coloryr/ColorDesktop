namespace ColorDesktop.AnalogClockPlugin;

public interface IClock
{
    void Update(AnalogClockConfigObj obj);
    void Tick();
}
