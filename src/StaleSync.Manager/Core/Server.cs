using System.Net;
using System.Net.Sockets;

namespace StaleSync.Manager.Core
{
    public sealed class Server
    {
        public static Server Instance { get; } = new();

        private TcpListener _readListener;
        private TcpListener _writeListener;

        private Server()
        {
        }

        public void Listen(Config config)
        {
            var readPort = config.ReadPort;
            _readListener = new TcpListener(IPAddress.Any, readPort);
            _readListener.Start();

            var writePort = config.WritePort;
            _writeListener = new TcpListener(IPAddress.Any, writePort);
            _writeListener.Start();

            Log.WriteLine($"Listening on ports {readPort} & {writePort}...");
        }
    }
}