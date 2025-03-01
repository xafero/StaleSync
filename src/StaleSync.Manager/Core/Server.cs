using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace StaleSync.Manager.Core
{
    public sealed class Server
    {
        public static Server Instance { get; } = new();

        private Config _config;

        private TcpListener _readListener;
        private TcpListener _writeListener;

        private Thread _readThread;
        private Thread _writeThread;

        private Dictionary<string, Session> _sessions;

        private Server()
        {
            _sessions = new Dictionary<string, Session>();
        }

        public bool ShouldRun { get; set; }

        public void Listen(Config config)
        {
            _config = config;
            ShouldRun = true;

            var readPort = config.ReadPort;
            _readListener = new TcpListener(IPAddress.Any, readPort);
            _readListener.Start();

            var writePort = config.WritePort;
            _writeListener = new TcpListener(IPAddress.Any, writePort);
            _writeListener.Start();

            _readThread = new Thread(ReadLoop) { IsBackground = true, Name = "ServerRead" };
            _readThread.Start();
            _writeThread = new Thread(WriteLoop) { IsBackground = true, Name = "ServerWrite" };
            _writeThread.Start();

            Log.WriteLine($"Listening on ports {readPort} & {writePort}...");
        }

        private void ReadLoop()
        {
            while (ShouldRun)
            {
                using var client = _readListener.AcceptTcpClient();
                var clientAdr = client.Client.RemoteEndPoint?.ToString();
                Log.WriteLine($"Client {clientAdr} connected read.");
            }
        }

        private void WriteLoop()
        {
            while (ShouldRun)
            {
                using var client = _writeListener.AcceptTcpClient();
                var clientAdr = client.Client.RemoteEndPoint?.ToString();
                Log.WriteLine($"Client {clientAdr} connected write.");
            }
        }
    }
}