using System.IO;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Threading;
using StaleSync.Proto;
using static StaleSync.Proto.CollTool;
using System;

// ReSharper disable InconsistentNaming

namespace StaleSync.Core
{
    public sealed class Client
    {
        public static Client Instance { get; } = new();

        private Config _config;

        private readonly Queue<Message> _readQueue;
        private Thread _readThread;

        private readonly Queue<Message> _writeQueue;
        private Thread _writeThread;

        private Client()
        {
            ClientId = ToTxt(Guid.NewGuid());
            _readQueue = new Queue<Message>();
            _writeQueue = new Queue<Message>();
        }

        public string ClientId { get; }
        public bool ShouldRun { get; set; }

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

        private static bool Read(StreamReader reader, out Message msg)
        {
            if (ReadLineTrim(reader) is { } json)
            {
                msg = JsonTool.FromJson<Message>(json);
                return true;
            }
            msg = null;
            return false;
        }

        private static bool Write(StreamWriter writer, Message msg)
        {
            if (JsonTool.ToJson(msg) is { } json)
            {
                writer.WriteLine(json);
                writer.Flush();
                return true;
            }
            return false;
        }

        public void Disconnect()
        {
            ShouldRun = false;
        }
    }
}