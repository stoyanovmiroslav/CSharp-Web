using SIS.WebServer.Routing;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SIS.WebServer
{
    public class Server
    {
        private const string LocalHostIpAddress = "127.0.0.1";

        private readonly int port;

        private readonly TcpListener tcpListener;

        private readonly ServerRoutingTable serverRoutingTable;

        private bool isRinning;

        public Server(int port, ServerRoutingTable serverRoutingTable)
        {
            this.port = port;
            this.serverRoutingTable = serverRoutingTable;

            this.tcpListener = new TcpListener(IPAddress.Parse(LocalHostIpAddress), this.port);
        }

        public void Run()
        {
            this.tcpListener.Start();
            this.isRinning = true;

            Console.WriteLine($"Server started at http//{LocalHostIpAddress}:{this.port}");
            while (isRinning)
            {
                Console.WriteLine("Waiting for client...");

                var client = tcpListener.AcceptSocketAsync().Result;

                Task.Run(() => ListenLoop(client));
            }
        }

        private async void ListenLoop(Socket client)
        {
            var connectionHandler = new ConnectionHandler(client, this.serverRoutingTable);
            await connectionHandler.ProcessRequestAsync();
        }
    }
}