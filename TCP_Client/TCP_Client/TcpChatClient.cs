using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace TCP_Client
{
    class TcpChatClient
    {
        string userName;
        private TcpClient client;
        private NetworkStream stream;
        private Thread threadReceive;

        public TcpChatClient(string name)
        {
            userName = name;
            client = new TcpClient();
            client.Connect(new IPEndPoint(IPAddress.Parse(Program.serverAddress), Program.port));
            stream = client.GetStream();
            SendMessage(name);
            threadReceive = new Thread(new ThreadStart(this.ReceiveMessage));
            threadReceive.Start();
        }

        private void ReceiveMessage()
        {
            try
            {
                while (true)
                {
                    byte[] data = new byte[64];
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));

                    } while (stream.DataAvailable);

                    string message = builder.ToString();
                    Console.WriteLine(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connection is interrupted. Exception: {0}", ex.Message);
                Console.ReadLine();
                Disconnect();
            }
        }

        public void SendMessage(string message)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }

        public void Disconnect()
        {
            if (stream != null)
                stream.Close();
            if (client != null)
                client.Close();
            if (threadReceive.IsAlive)
                threadReceive.Abort();
        }
    }
}
