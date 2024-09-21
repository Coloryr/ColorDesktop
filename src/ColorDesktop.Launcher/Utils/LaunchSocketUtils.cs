using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ColorDesktop.Launcher.Helper;
using DotNetty.Buffers;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;

namespace ColorDesktop.Launcher.Utils;

public static class LaunchSocketUtils
{
    public static int Port { get; private set; }

    private const int TypeLaunchShow = 1;

    private static IEventLoopGroup _bossGroup;
    private static IEventLoopGroup _workerGroup;
    private static ServerBootstrap _bootstrap;

    private static IChannel _channel;

    /// <summary>
    /// 客户端信道
    /// </summary>
    private static readonly List<IChannel> _channels = [];

    /// <summary>
    /// 获取所有正在使用的端口
    /// </summary>
    /// <returns>端口列表</returns>
    private static List<int> PortIsUsed()
    {
        var ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
        var ipsTCP = ipGlobalProperties.GetActiveTcpListeners();
        var ipsUDP = ipGlobalProperties.GetActiveUdpListeners();
        var tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

        var allPorts = new List<int>();
        foreach (var ep in ipsTCP) allPorts.Add(ep.Port);
        foreach (var ep in ipsUDP) allPorts.Add(ep.Port);
        foreach (var conn in tcpConnInfoArray) allPorts.Add(conn.LocalEndPoint.Port);

        return allPorts;
    }

    /// <summary>
    /// 获取一个没有使用的端口
    /// </summary>
    /// <returns>端口</returns>
    private static int GetFirstAvailablePort()
    {
        try
        {
            var portUsed = PortIsUsed();
            if (portUsed.Count > 5000)
            {
                return -1;
            }
            var random = new Random();
            do
            {
                int temp = random.Next() % 65535;
                if (!portUsed.Contains(temp))
                {
                    return temp;
                }
            }
            while (true);
        }
        catch
        {
            var random = new Random();
            do
            {
                try
                {
                    int port = random.Next(65535);
                    using var socket = new TcpListener(IPAddress.Any, port);
                    socket.Start();
                    socket.Stop();
                    return port;
                }
                catch
                {

                }
            } while (true);
        }
    }

    /// <summary>
    /// 启动游戏端口服务器
    /// </summary>
    /// <returns></returns>
    private static async Task<int> RunServerAsync()
    {
        if (_channel != null)
        {
            return (_channel.LocalAddress as IPEndPoint)!.Port;
        }
        _bossGroup = new MultithreadEventLoopGroup(1);
        _workerGroup = new MultithreadEventLoopGroup();
        try
        {
            _bootstrap = new();
            _bootstrap.Group(_bossGroup, _workerGroup);
            _bootstrap.Channel<TcpServerSocketChannel>();
            _bootstrap
                .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
                {
                    channel.Pipeline.AddLast("colormc", new ServerHandler());
                }));

            int port = GetFirstAvailablePort();
            _channel = await _bootstrap.BindAsync(IPAddress.Any, port);

            return port;
        }
        catch (Exception e)
        {
            PathHelper.OpenFileWithExplorer(Logs.Crash("netty error", e));
            return 0;
        }
    }

    /// <summary>
    /// 停止游戏端口服务器
    /// </summary>
    public static async void Stop()
    {
        await _channel.CloseAsync();
        await Task.WhenAll(
                _bossGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)),
                _workerGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)));
    }

    /// <summary>
    /// 发送数据到所有客户端
    /// </summary>
    /// <param name="byteBuffer"></param>
    private static void SendMessage(IByteBuffer byteBuffer)
    {
        foreach (var item in _channels.ToArray())
        {
            try
            {
                if (item.IsWritable)
                {
                    item.WriteAndFlushAsync(byteBuffer.RetainedDuplicate());
                }
            }
            catch
            {

            }
        }
    }

    public static async Task SendMessage(int port)
    {
        if (port <= 0)
        {
            return;
        }
        var group = new MultithreadEventLoopGroup();
        try
        {
            var bootstrap = new Bootstrap();
            bootstrap
                .Group(group)
                .Channel<TcpSocketChannel>()
                .Option(ChannelOption.TcpNodelay, true)
                .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                {
                    IChannelPipeline pipeline = channel.Pipeline;

                }));

            IChannel clientChannel = await bootstrap.ConnectAsync(IPAddress.Parse("127.0.0.1"), port);

            var buf = Unpooled.Buffer();
            buf.WriteInt(TypeLaunchShow);
            await clientChannel.WriteAndFlushAsync(buf);

            await clientChannel.CloseAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            await group.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1));
        }
    }

    public static async Task Init()
    {
        Port = await RunServerAsync();
    }

    private static string ReadString(this IByteBuffer buf)
    {
        int size = buf.ReadInt();
        var datas = new byte[size];
        buf.ReadBytes(datas);
        return Encoding.UTF8.GetString(datas);
    }

    private class ServerHandler : ChannelHandlerAdapter
    {
        public override void ChannelActive(IChannelHandlerContext ctx)
        {
            _channels.Add(ctx.Channel);
        }

        public override void ChannelInactive(IChannelHandlerContext ctx)
        {
            _channels.Remove(ctx.Channel);
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            if (message is IByteBuffer buffer)
            {
                try
                {
                    int type = buffer.ReadInt();

                    if (type == TypeLaunchShow)
                    {
                        App.ShowMainWindow();
                    }
                }
                catch
                {

                }
            }
        }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            context.CloseAsync();
        }
    }

    private static string[] ReadStringList(this IByteBuffer buf)
    {
        int size = buf.ReadInt();
        string[] temp = new string[size];
        for (int a = 0; a < size; a++)
        {
            temp[a] = buf.ReadString();
        }

        return temp;
    }

    private static IByteBuffer WriteString(this IByteBuffer buf, string data)
    {
        var temp = Encoding.UTF8.GetBytes(data);
        buf.WriteInt(temp.Length);
        buf.WriteBytes(temp);

        return buf;
    }

    private static IByteBuffer WriteStringList(this IByteBuffer buf, string[] data)
    {
        buf.WriteInt(data.Length);
        foreach (var item in data)
        {
            buf.WriteString(item);
        }

        return buf;
    }
}
