using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace TCP_Server
{
    class Program
    {
        static int port = 8080;
        static string ReadCommand()
        {
            Console.Write("$ ");
            return Console.ReadLine().ToLower();
        }
        static void Main(string[] args)
        {
            TcpChatServer server = new TcpChatServer(new IPEndPoint(IPAddress.Parse("127.0.0.1"), port));

            string command;
            while ((command = ReadCommand()) != "exit")
            {
                switch (command)
                {
                    case "start":
                        server.Start();
                        server.AcceptClientsAsync();
                        Console.WriteLine("Listening connections on port {1}", port);
                        break;

                    case "stop":
                        Console.WriteLine("Stopping listener.");
                        server.Stop();
                        break;

                    case "exit":
                        Console.WriteLine("Stopping listener.");
                        server.Stop();
                        server.CloseClientsAsync();
                        break;
                    
                    
                }
            }
        }
    }
}
