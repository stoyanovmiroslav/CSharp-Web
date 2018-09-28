using SIS.WebServer.Routing;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
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

            var task = Task.Run(() => ListenLoop());
            task.Wait();
        }

        public async Task ListenLoop()
        {
            while (this.isRinning)
            {
                var client = await this.tcpListener.AcceptSocketAsync();
                var connectionHandler = new ConnectionHandler(client, this.serverRoutingTable);
                var responseTask = connectionHandler.ProcessRequestAsync();
                responseTask.Wait();
            }
        }
    }
}