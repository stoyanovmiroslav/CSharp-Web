using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using SIS.WebServer.Api.Contracts;
using SIS.WebServer.Routing;

namespace SIS.WebServer
{
    public class Server
    {
        private const string LocalhostIpAddress = "127.0.0.1";

        private readonly int port;

        private readonly TcpListener listener;

        private readonly IHttpHandler httpHandler;

        private bool isRunning;

        public Server(int port, IHttpHandler httpHandler)
        {
            this.port = port;
            this.listener = new TcpListener(IPAddress.Parse(LocalhostIpAddress), port);
            this.httpHandler = httpHandler;
        }

        public void Run()
        {
            this.listener.Start();
            this.isRunning = true;

            Console.WriteLine($"Server started at http://{LocalhostIpAddress}:{this.port}");
            while (isRunning)
            {
                Console.WriteLine("Waiting for client...");

                var client = listener.AcceptSocketAsync().GetAwaiter().GetResult();

                Task.Run(() => Listen(client));
            }
        }

        public async void Listen(Socket client)
        { 
            var connectionHandler = new ConnectionHandler(client, this.httpHandler);
            await connectionHandler.ProcessRequestAsync();
        }
    }
}