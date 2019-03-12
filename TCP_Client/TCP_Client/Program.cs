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
        static int port = 8080;
        static string serverAddress = "127.0.0.1";
        static void Main(string[] args)
        {
            try
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(serverAddress), port);

                Socket connectionSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                connectionSocket.Connect(ipPoint);
                Console.Write("Insert a message here:");
                string message = Console.ReadLine();
                byte[] data = Encoding.Unicode.GetBytes(message);
                connectionSocket.Send(data);

                data = new byte[256];
                StringBuilder builder = new StringBuilder();
                int bytes = 0;
                do
                {
                    bytes = connectionSocket.Receive(data);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (connectionSocket.Available > 0);
                Console.WriteLine("Response: " + builder.ToString());

                connectionSocket.Shutdown(SocketShutdown.Both);
                connectionSocket.Close();

                Console.WriteLine("Socket is closed.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
