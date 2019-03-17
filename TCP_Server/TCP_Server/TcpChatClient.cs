using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace TCP_Server
{
    class TcpChatClient
    {
        public string name;
        public Guid id;
        private NetworkStream stream;
        private TcpClient client;
        private TcpChatServer server;

        public TcpChatClient(TcpClient tcpClient, TcpChatServer tcpServer)
        {
            this.id = Guid.NewGuid();
            this.client = tcpClient;
            this.stream = client.GetStream();
            this.name = GetMessage();
            this.server = tcpServer;
        }

        public async void SendNotification(TcpChatClient sender, string message)
        {
            if (this != sender)
            {

            }
        }

        private string GetMessage()
        {
            byte[] data = new byte[64]; // буфер для получаемых данных
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (stream.DataAvailable);

            return builder.ToString();
        }

        public async void ReceiveMessagesAsync()
        {

        }
    }
}
