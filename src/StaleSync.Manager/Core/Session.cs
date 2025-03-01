using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using StaleSync.Proto;

namespace StaleSync.Manager.Core
{
    public sealed class Session
    {
        public Queue<Message> ReadQueue { get; } = new();
        public Queue<Message> WriteQueue { get; } = new();

        public TcpClient ClientR { get; private set; }
        public StreamReader ReaderR { get; private set; }
        public StreamWriter WriterR { get; private set; }

        public TcpClient ClientW { get; private set; }
        public StreamReader ReaderW { get; private set; }
        public StreamWriter WriterW { get; private set; }

        public void Set(ConnKind kind, TcpClient client, StreamReader reader, StreamWriter writer)
        {
            if (kind == ConnKind.Read)
            {
                ClientR = client;
                ReaderR = reader;
                WriterR = writer;
            }
            else if (kind == ConnKind.Write)
            {
                ClientW = client;
                ReaderW = reader;
                WriterW = writer;
            }
        }
    }
}