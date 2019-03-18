using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace UDP_Server
{
    class Program
    {
        static string address = "127.0.0.1";
        static int port = 8080;

        static string ReadCommand()
        {
            //Console.Write("$ ");
            return Console.ReadLine().ToLower();
        }

        static void Main(string[] args)
        {
            UdpChatServer server = new UdpChatServer(port);
            server.Start();
            Console.WriteLine("Listening for connections on port {0}.", port);
            string command;
            while ((command = ReadCommand()) != "exit")
            {
                //switch (command)
                //{
                    
                //}
            }
            server.Stop();



        }
    }
}
