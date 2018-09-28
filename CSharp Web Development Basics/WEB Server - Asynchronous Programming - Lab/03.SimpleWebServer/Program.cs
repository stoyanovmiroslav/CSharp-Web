using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace _03.SimpleWebServer
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress iPAddress = IPAddress.Parse("127.0.0.1");
            int port = 80;

            TcpListener tcpListener = new TcpListener(iPAddress, port);
            tcpListener.Start();

            Console.WriteLine("Server started");
            Console.WriteLine($"Listening to TCP clients at {iPAddress}:{port}");

            while (true)
            {
                Console.WriteLine("Waiting for client...");

                var client = tcpListener.AcceptTcpClient();

                Task.Run(() => ConnectWithTcpClient(client));
            }
        }


        private static async void ConnectWithTcpClient(TcpClient client)
        {
            Console.WriteLine("Client connected.");

            var stream = client.GetStream();

            var buffer = new Byte[1024];

            int redStreamLength = await stream.ReadAsync(buffer, 0, buffer.Length);

            string request = Encoding.UTF8.GetString(buffer, 0, redStreamLength);

            Console.WriteLine(new string('=', 60));
            Console.WriteLine(request);

            var response = Encoding.UTF8.GetBytes("HTTP/1.0 200 OK" + Environment.NewLine + 
                                                  "Content-Length: 20" + Environment.NewLine +
                                                  "Content-Type: text/html" + Environment.NewLine +
                                                  Environment.NewLine + 
                                                  "<h1>Hello from server!<h1>");

            await stream.WriteAsync(response, 0, response.Length);

            stream.Dispose();

            Console.WriteLine("Connection closed.");
        }
    }
}