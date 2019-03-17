using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace TCP_Server
{
    delegate void Notification(string message, TcpChatClient sender);

    class TcpChatServer: TcpListener
    {
        public List<TcpChatClient> clients;

        public TcpChatServer(IPEndPoint localEP)
            : base (localEP)
        {
            clients = new List<TcpChatClient>();
        }
        
        //public async void AcceptClientsAsync()
        //{
        //    try
        //    {
        //        while (this.Active)
        //        {
        //            TcpChatClient newClient = new TcpChatClient(await this.AcceptTcpClientAsync(), this);
        //            this.clients.Add(newClient);
        //            // this.notifyEveryone += newClient.SendNotification;
        //            newClient.ReceiveMessagesAsync();
        //        }
        //    }
       
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }
        //}
        
        public void RemoveConnection(Guid id)
        {
            TcpChatClient client = clients.FirstOrDefault(c => c.id == id);
            if (client != null)
                clients.Remove(client);
        }

        public void Listen()
        {
            try
            {
                while (true)
                {
                    TcpChatClient newClient = new TcpChatClient(this.AcceptTcpClient(), this);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Disconnect();
            }
        }

        public void BroadcastMessage(string message, Guid senderId)
        {
            foreach (TcpChatClient client in clients)
                if (senderId != client.id)
                    client.SendMessageAsync(message);
        }

        public void Disconnect()
        {
            this.Stop();

            foreach (TcpChatClient client in clients)
                client.Close();
        }

    }
}
