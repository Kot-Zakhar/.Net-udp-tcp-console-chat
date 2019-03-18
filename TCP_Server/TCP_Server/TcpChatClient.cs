using System;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace TCP_Server
{
    class TcpChatClient
    {
        public string name;
        public Guid id;
        public Thread connectionThread;
        public int port { get; private set; }
        private NetworkStream stream;
        private TcpClient client;
        private TcpChatServer server;


        public TcpChatClient(TcpClient tcpClient, TcpChatServer tcpServer)
        {
            this.id = Guid.NewGuid();
            this.client = tcpClient;
            this.stream = client.GetStream();
            this.server = tcpServer;
            this.port = ((IPEndPoint)client.Client.RemoteEndPoint).Port;

            this.connectionThread = new Thread(new ThreadStart(this.Process));
            this.connectionThread.Start();
        }

        public async void SendMessageAsync(string message)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            await this.stream.WriteAsync(data, 0, data.Length);
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

        public void Process()
        {
            this.name = GetMessage();
            string message = this.name + String.Format(" is connected (port: {0}).", this.port);
            server.BroadcastMessage(message, this.id);
            //Console.WriteLine(message);

            try
            {
                while (true)
                {
                    message = GetMessage();
                    message = String.Format("{0}: {1}", this.name, message);
                    //Console.WriteLine(message);
                    server.BroadcastMessage(message, this.id);
                }
            }
            catch
            {
                message = String.Format("{0} left the chat.", this.name);
                //Console.WriteLine(message);
                server.BroadcastMessage(message, this.id);
            }
            finally
            {
                server.RemoveConnection(this.id);
                this.Close();
            }

        }

        public void Close()
        {
            if (stream != null)
                stream.Close();
            if (client != null)
                client.Close();
            if (connectionThread.IsAlive)
                connectionThread.Abort();
        }

    }
}
