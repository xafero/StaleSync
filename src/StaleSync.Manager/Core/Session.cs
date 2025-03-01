using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using StaleSync.Proto;
using System.Threading;
using static StaleSync.Proto.CollTool;
using static StaleSync.Proto.Protocol;

// ReSharper disable InconsistentNaming

namespace StaleSync.Manager.Core
{
    public sealed class Session
    {
        private Config _config;

        private readonly Queue<Message> _readQueue;
        private Thread _readThread;

        private readonly Queue<Message> _writeQueue;
        private Thread _writeThread;

        public Session()
        {
            _readQueue = new Queue<Message>();
            _writeQueue = new Queue<Message>();
        }

        public bool ShouldRun { get; set; }

        public void Start(Config config)
        {
            _config = config;
            ShouldRun = true;
            if (ReaderR != null)
            {
                _readThread = new Thread(ReadLoop) { IsBackground = true };
                _readThread.Start();
            }
            if (WriterW != null)
            {
                _writeThread = new Thread(WriteLoop) { IsBackground = true };
                _writeThread.Start();
            }
        }

        public void Stop()
        {
            ShouldRun = false;
        }

        private void ReadLoop()
        {
            while (ShouldRun)
            {
                if (Read(ReaderR, out var msg))
                    _readQueue.Enqueue(msg);
            }
            _readQueue.Clear();
        }

        private void WriteLoop()
        {
            var poll = _config.Poll;

            while (ShouldRun)
            {
                if (TryDequeue(_writeQueue) is { } msg)
                    if (Write(WriterW, msg))
                        continue;
                Thread.Sleep(poll);
            }
            _writeQueue.Clear();
        }

        public TcpClient ClientR { get; private set; }
        public StreamReader ReaderR { get; private set; }

        public TcpClient ClientW { get; private set; }
        public StreamWriter WriterW { get; private set; }

        public void Set(ConnKind kind, TcpClient client, StreamReader reader, StreamWriter writer)
        {
            if (kind == ConnKind.Read)
            {
                ClientR = client;
                ReaderR = reader;
            }
            else if (kind == ConnKind.Write)
            {
                ClientW = client;
                WriterW = writer;
            }
        }
    }
}