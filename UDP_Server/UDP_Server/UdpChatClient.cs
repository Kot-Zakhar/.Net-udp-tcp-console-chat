using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;

namespace UDP_Server
{
    class UdpChatClient
    {
        public Guid id { get; private set; }
        public IPEndPoint ipEndPoint { get; private set; }
        public string name;

        public UdpChatClient(IPEndPoint endPoint)
        {
            ipEndPoint = endPoint;
            id = Guid.NewGuid();
        }
    }
}
