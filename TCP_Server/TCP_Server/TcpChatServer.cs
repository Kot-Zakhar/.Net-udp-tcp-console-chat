using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace TCP_Server
{
    delegate void Notification(TcpChatClient Sender, string Message);

    class TcpChatServer: TcpListener
    {
        public List<TcpChatClient> clients;
        public event Notification notifyEveryone;

        public TcpChatServer(IPEndPoint localEP)
            : base (localEP)
        {
            clients = new List<TcpChatClient>();
        }
        
        public async void AcceptClientsAsync()
        {
            try
            {
                while (this.Active)
                {
                    TcpChatClient newClient = new TcpChatClient(await this.AcceptTcpClientAsync(), this);
                    this.clients.Add(newClient);
                    this.notifyEveryone += newClient.SendNotification;
                    newClient.ReceiveMessagesAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async void CloseClientsAsync()
        {

        }

    }
}
