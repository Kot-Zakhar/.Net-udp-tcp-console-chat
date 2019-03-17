using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace TCP_Client
{
    class Program
    {
        static public int port = 8080;
        static public string serverAddress = "127.0.0.1";
        static void Main(string[] args)
        {
            Console.Write("Insert your name: ");
            TcpChatClient client = new TcpChatClient(Console.ReadLine());
            try
            {
                while (true)
                    client.SendMessage(Console.ReadLine());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connection has been interrupted. Exception: {0}", ex.Message);
            }
            finally
            {
                client.Disconnect();
            }
            
        }
    }
}
