using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using StaleSync.Proto;
using static StaleSync.Proto.CollTool;
using static StaleSync.Proto.Protocol;

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

        private readonly Dictionary<string, Session> _sessions;
        private readonly object _sessionLock = new();

        private Server()
        {
            ServerId = ToTxt(Guid.NewGuid());
            _sessions = new Dictionary<string, Session>();
        }

        public string ServerId { get; }
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
                var client = _readListener.AcceptTcpClient();
                var clientAdr = client.Client.RemoteEndPoint?.ToString();
                Log.WriteLine($"Client {clientAdr} connected read.");

                var readStream = client.GetStream();
                var reader = new StreamReader(readStream);
                Read(reader, out var idc);

                if (idc.Payload is not Announce ma || TrimOrNull(ma.Id) is not { } key)
                {
                    client.Dispose();
                    continue;
                }

                var writer = new StreamWriter(readStream);
                var idm = Wrap(Announce(ServerId, CommKind.Server));
                Write(writer, idm);

                var session = GetSessionOrCreate(key);
                session.Set(ConnKind.Read, client, reader, writer);
                session.Start(_config);
            }
        }

        private void WriteLoop()
        {
            while (ShouldRun)
            {
                var client = _writeListener.AcceptTcpClient();
                var clientAdr = client.Client.RemoteEndPoint?.ToString();
                Log.WriteLine($"Client {clientAdr} connected write.");

                var writeStream = client.GetStream();
                var reader = new StreamReader(writeStream);
                Read(reader, out var idc);

                if (idc.Payload is not Announce ma || TrimOrNull(ma.Id) is not { } key)
                {
                    client.Dispose();
                    continue;
                }

                var writer = new StreamWriter(writeStream);
                var idm = Wrap(Announce(ServerId, CommKind.Server));
                Write(writer, idm);

                var session = GetSessionOrCreate(key);
                session.Set(ConnKind.Write, client, reader, writer);
                session.Start(_config);
            }
        }

        private Session GetSessionOrCreate(string key)
        {
            lock (_sessionLock)
            {
                if (_sessions.TryGetValue(key, out var value))
                    return value;
                _sessions.Add(key, value = new Session());
                return value;
            }
        }
    }
}