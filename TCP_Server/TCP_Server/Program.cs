using System;
using System.Threading;
using System.Net;

namespace TCP_Server
{
    class Program
    {
        static int port = 8080;

        static string ReadCommand()
        {
            Console.Write("$ ");
            return Console.ReadLine().ToLower();
        }

        static void Main(string[] args)
        {
            TcpChatServer server = new TcpChatServer(new IPEndPoint(IPAddress.Parse("127.0.0.1"), port));
            Thread listenThread = new Thread(new ThreadStart(server.Listen));

            string command;
            try
            {
                while ((command = ReadCommand()) != "exit")
                {
                    switch (command)
                    {
                        case "start":
                            server.Start(); // server starts listening the connections on $port
                            listenThread.Start();
                            //server.AcceptClientsAsync();
                            Console.WriteLine("Listening connections on port {0}", port);
                            break;

                        case "stop":
                            Console.WriteLine("Stopping listener.");
                            server.Stop();
                            listenThread.Abort();// if all connections are crushing, means that all child threads are aborted too
                            break;                                      
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Server has been stopped. Exception: {0}", ex.Message);
            }
            finally
            {
                Console.WriteLine("Stopping listener.");
                server.Disconnect();
            }
        }
    }
}
