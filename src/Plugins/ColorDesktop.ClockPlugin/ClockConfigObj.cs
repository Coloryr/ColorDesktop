namespace ColorDesktop.ClockPlugin;

public record ClockConfigObj
{
    public bool Ntp { get; set; }
    public string NtpIp { get; set; }
    public int NtpUpdateTime { get; set; }
    public int TimeZone { get; set; }
}
