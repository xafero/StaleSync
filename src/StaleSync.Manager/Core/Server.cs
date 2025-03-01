using System.Net;
using System.Net.Sockets;

namespace StaleSync.Manager.Core
{
    public static class Server
    {
        private static TcpListener _readListener;
        private static TcpListener _writeListener;

        public static void Start(int writePort, int readPort)
        {
            _readListener = new TcpListener(IPAddress.Any, readPort);
            _writeListener = new TcpListener(IPAddress.Any, writePort);

            _readListener.Start();
            _writeListener.Start();

            Log.WriteLine($"Listening on ports {readPort} & {writePort}...");

        // TODO
        
            /*using var client = listener.AcceptTcpClient();
            var clientAdr = client.Client.RemoteEndPoint?.ToString();
            Log.WriteLine($"Client {clientAdr} connected.");

            using var stream = client.GetStream();
            using var reader = new StreamReader(stream);
            using var writer = new StreamWriter(stream);
            writer.AutoFlush = true;
            Log.WriteLine("Connection established!");

            while (true)
            {
                Console.Write("Command> ");
                var command = Console.ReadLine();
                if (string.IsNullOrEmpty(command)) break;

                writer.WriteLine(command);
                var response = reader.ReadLine();
                Console.WriteLine(response);
            }*/
        }
    }
}