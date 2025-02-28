using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace StaleSync.Manager.Core
{
    public static class Server
    {
        public static void Start(int port = 4444)
        {
            var listener = new TcpListener(IPAddress.Any, port);

            listener.Start();
            Console.WriteLine("Listening on port " + port + "...");

            using var client = listener.AcceptTcpClient();
            using var stream = client.GetStream();
            using var reader = new StreamReader(stream);
            using var writer = new StreamWriter(stream);
            writer.AutoFlush = true;
            Console.WriteLine("Connection established!");

            while (true)
            {
                Console.Write("Command> ");
                var command = Console.ReadLine();
                if (string.IsNullOrEmpty(command)) break;

                writer.WriteLine(command);
                var response = reader.ReadLine();
                Console.WriteLine(response);
            }
        }
    }
}