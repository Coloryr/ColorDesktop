namespace ColorDesktop.AnalogClockPlugin;

public interface IClock
{
    void Update(AnalogClockInstanceConfigObj obj);
    void Tick();
}
