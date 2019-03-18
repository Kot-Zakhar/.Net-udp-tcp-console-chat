using System;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace UDP_Client
{
    class UdpChatClient: UdpClient
    {
        public string name;
        public Thread receiveThread;
        private PrintMessageDelegate PrintMessage;
        private IPEndPoint remoteEndPoint;

        public UdpChatClient (IPEndPoint ipEndPoint, PrintMessageDelegate printMessageDelegate) : base ()
        {
            PrintMessage = printMessageDelegate;
            try
            {
                this.Connect(ipEndPoint);
                remoteEndPoint = ipEndPoint;
                receiveThread = new Thread(new ThreadStart(ProcessIncomeMessages));
                receiveThread.Start();
            }
            catch (Exception e)
            {
                PrintMessage(e.Message);
            }
        }

        private void ProcessIncomeMessages()
        {
            try
            {
                while (true)
                {
                    byte[] data = this.Receive(ref this.remoteEndPoint);
                    string message = Encoding.Unicode.GetString(data);
                    PrintMessage(message);
                }
            }
            catch (Exception e)
            {
                PrintMessage(e.Message);
            }
        }

    }
}
