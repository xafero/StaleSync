using System.IO;
using System.Net.Sockets;
using System.Threading;
using StaleSync.Proto;
using static StaleSync.Proto.CollTool;
using System;
using static StaleSync.Proto.Protocol;

// ReSharper disable InconsistentNaming

namespace StaleSync.Core
{
    public sealed class Client
    {
        public static Client Instance { get; } = new();

        private Config _config;

        private readonly EvQueue<Message> _readQueue;
        private Thread _readThread;

        private readonly EvQueue<Message> _writeQueue;
        private Thread _writeThread;

        private Client()
        {
            ClientId = ToTxt(Guid.NewGuid());
            _readQueue = new EvQueue<Message>();
            _writeQueue = new EvQueue<Message>();
        }

        public string ClientId { get; }
        public string ServerId { get; private set; }
        public bool ShouldRun { get; set; }

        public event EventHandler<ConnectedArgs> Connected; 

        public void Connect(Config config)
        {
            _config = config;
            ShouldRun = true;
            _readThread = new Thread(ReadLoop) { IsBackground = true, Name = "ClientRead" };
            _readThread.Start();
            _writeThread = new Thread(WriteLoop) { IsBackground = true, Name = "ClientWrite" };
            _writeThread.Start();
        }

        private void ReadLoop()
        {
            var serverIP = _config.HostIP;
            var readPort = _config.HostRead;
            var delay = _config.Reconnect;

            while (ShouldRun)
            {
                try
                {
                    using var readClient = new TcpClient(serverIP, readPort);
                    using var readStream = readClient.GetStream();
                    using var reader = new StreamReader(readStream);

                    using var writer = new StreamWriter(readStream);
                    var idm = Wrap(Announce(ClientId, CommKind.Client));
                    Write(writer, idm);

                    Read(reader, out var idc);
                    if (idc.Payload is not Announce ma || TrimOrNull(ma.Id) is not { } key)
                        continue;
                    ServerId = key;
                    OnConnected(ConnKind.Read);

                    while (ShouldRun)
                    {
                        if (Read(reader, out var msg))
                            _readQueue.Enqueue(msg);
                    }
                }
                catch (SocketException)
                {
                    Thread.Sleep(delay);
                }
            }
            _readQueue.Clear();
        }

        private void WriteLoop()
        {
            var serverIP = _config.HostIP;
            var writePort = _config.HostWrite;
            var delay = _config.Reconnect;
            var poll = _config.Poll;

            while (ShouldRun)
            {
                try
                {
                    using var writeClient = new TcpClient(serverIP, writePort);
                    using var writeStream = writeClient.GetStream();
                    using var writer = new StreamWriter(writeStream);

                    using var reader = new StreamReader(writeStream);
                    var idm = Wrap(Announce(ClientId, CommKind.Client));
                    Write(writer, idm);

                    Read(reader, out var idc);
                    if (idc.Payload is not Announce ma || TrimOrNull(ma.Id) is not { } key)
                        continue;
                    ServerId = key;
                    OnConnected(ConnKind.Write);

                    while (ShouldRun)
                    {
                        if (TryDequeue(_writeQueue) is { } msg)
                            if (Write(writer, msg))
                                continue;
                        Thread.Sleep(poll);
                    }
                }
                catch (SocketException)
                {
                    Thread.Sleep(delay);
                }
            }
            _writeQueue.Clear();
        }

        private void OnConnected(ConnKind kind)
        {
            Connected?.Invoke(this, new ConnectedArgs
            {
                ServerId = ServerId, Kind = kind
            });
        }

        public void Disconnect()
        {
            ShouldRun = false;
        }
    }
}