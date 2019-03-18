using System;
using System.Threading;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace UDP_Client
{
    delegate void PrintMessageDelegate(string message);
    class Program
    {
        static int port = 8080;
        static string ipAddress = "127.0.0.1";
        static void Main(string[] args)
        {
            // maybe another constructor need to be used - with port = 0
            // probably, current constructor doesn't bind socket to a port, and server wouldn't be able to identify user
            UdpChatClient client = new UdpChatClient(new IPEndPoint(IPAddress.Parse(ipAddress), port), Console.WriteLine);

            Console.Write("Insert your name: ");
            client.name = Console.ReadLine();
            byte[] data = Encoding.Unicode.GetBytes(client.name);
            client.Send(data, data.Length);

            while (true)
            {
                string message = Console.ReadLine();
                data = Encoding.Unicode.GetBytes(message);
                client.Send(data, data.Length);
            }

        }
    }
}
