using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace UDP_Server
{
    class UdpChatServer
    {
        private UdpClient server;
        private List<UdpChatClient> clients;
        private Thread receiveThread;

        public UdpChatServer (int localPort)
        {
            server = new UdpClient(localPort);
            clients = new List<UdpChatClient>();
            receiveThread = new Thread(new ThreadStart(this.ProcessIncomeMessages));
        }

        public void Start()
        {
            receiveThread.Start();
        }

        public void ProcessIncomeMessages()
        {
            IPEndPoint anyEndPoint = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                byte[] data = server.Receive(ref anyEndPoint);
                string message = Encoding.Unicode.GetString(data);
                UdpChatClient currentClient = clients.FirstOrDefault(c => c.ipEndPoint.Port == anyEndPoint.Port); // it's is wrong equation. In global net Address need to be checked too
                if (currentClient != null)
                {
                    //message = String.Format("{0}: {1}: {2}", currentClient.ipEndPoint.Port, currentClient.name, message);
                    message = String.Format("{0} ({1}): {2}", currentClient.name, DateTime.Now.ToShortTimeString(), message);
                }
                else
                {
                    currentClient = new UdpChatClient(anyEndPoint);
                    currentClient.name = message;
                    clients.Add(currentClient);
                    message = String.Format("{0} (port: {1}) is connected.", message, currentClient.ipEndPoint.Port);
                }
                Console.WriteLine(message);
                this.BroadcastMessage(message, currentClient.id);
            }
        }

        public async void BroadcastMessage(string message, Guid id)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            foreach (UdpChatClient client in clients)
                if (client.id != id)
                    await server.SendAsync(data, data.Length, client.ipEndPoint);
        }

        public void Stop()
        {
            server.Close();
            receiveThread.Abort();
            clients.Clear();
        }


    }
}
