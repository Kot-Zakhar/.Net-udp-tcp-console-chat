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
        static void Main(string[] args)
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                StringBuilder builder = new StringBuilder();

                listenSocket.Bind(ipPoint);

                listenSocket.Listen(10);

                Console.WriteLine("Server is on. Waiting for a connection");

                for (int i = 0; i < 10; i++)
                {
                    Socket handler = listenSocket.Accept();
                    int bytes = 0;
                    byte[] data = new byte[256];
                    builder.Clear();
                    do
                    {
                        bytes = handler.Receive(data);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    } while (handler.Available > 0);

                    Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + builder.ToString());

                    data = Encoding.Unicode.GetBytes("Your message is delivered.");
                    handler.Send(data);

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }

                Console.WriteLine("Socket is closed.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadLine();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
