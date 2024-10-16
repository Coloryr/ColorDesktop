using System.Net.Sockets;

namespace ColorDesktop.ClockPlugin;

/// <summary>
/// Static class to receive the time from a NTP server.
/// </summary>
public static class NtpClient
{
    private static Timer s_timer;
    private static int s_count = 0;
    public static DateTime Date { get; private set; }

    public static void Start()
    {
        s_timer = new(TimerTick);
        s_timer.Change(TimeSpan.Zero, TimeSpan.FromSeconds(1));
        Task.Run(async () =>
        {
            bool res = false;
            do
            {
                res = await GetTime();
                Thread.Sleep(1000);
            }
            while (!res);
        });
    }

    public static void Stop()
    {
        s_timer.Dispose();
    }

    private static void TimerTick(object? sender)
    {
        Date = Date.AddSeconds(1.0);
        s_count++;
        if (ClockPlugin.Config.NtpUpdateTime > 0 && s_count >= ClockPlugin.Config.NtpUpdateTime)
        {
            s_count = 0;

            GetTime();
        }
    }

    public static Task<bool> GetTime()
    {
        return Task.Run(() =>
        {
            try
            {
                if (!ClockPlugin.Config.Ntp || string.IsNullOrWhiteSpace(ClockPlugin.Config.NtpIp))
                {
                    return false;
                }
                Date = GetNetworkTime(ClockPlugin.Config.NtpIp);
                Date = Date.AddHours(ClockPlugin.Config.TimeZone);
                return true;
            }
            catch
            {
                return false;
            }
        });
    }

    /// <summary>
    /// Gets the current DateTime form <paramref name="ep"/> IPEndPoint.
    /// </summary>
    /// <param name="ep">The IPEndPoint to connect to.</param>
    /// <returns>A DateTime containing the current time.</returns>
    public static DateTime GetNetworkTime(string ntpServer)
    {
        using var s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        s.Connect(ntpServer, 123);

        byte[] ntpData = new byte[48]; // RFC 2030 
        ntpData[0] = 0x1B;
        for (int i = 1; i < 48; i++)
            ntpData[i] = 0;

        s.Send(ntpData);
        s.Receive(ntpData);

        byte offsetTransmitTime = 40;
        ulong intpart = 0;
        ulong fractpart = 0;

        for (int i = 0; i <= 3; i++)
            intpart = 256 * intpart + ntpData[offsetTransmitTime + i];

        for (int i = 4; i <= 7; i++)
            fractpart = 256 * fractpart + ntpData[offsetTransmitTime + i];

        ulong milliseconds = intpart * 1000 + (fractpart * 1000) / 0x100000000L;
        s.Close();

        var timeSpan = TimeSpan.FromTicks((long)milliseconds * TimeSpan.TicksPerMillisecond);

        var dateTime = new DateTime(1900, 1, 1);
        dateTime += timeSpan;

        return dateTime;
    }
}
