using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace WEB_Server___HTTP_Protocol___Lab
{
    public class HttpServer : IHttpServer
    {
        private bool isRunning = true;
        TcpListener tcpListener;

        public HttpServer()
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");

            Console.WriteLine("Starting TCP listener...");

            tcpListener = new TcpListener(ipAddress, 80);
        }

        public void Start()
        {
            tcpListener.Start();

            while (this.isRunning)
            {
                var client = tcpListener.AcceptTcpClient();
                var buffer = new byte[10240];
                var stream = client.GetStream();
                var readLength = stream.Read(buffer, 0, buffer.Length);
                var requestText = Encoding.UTF8.GetString(buffer, 0, readLength);

                Console.WriteLine(requestText);

                var responseText = File.ReadAllText("index.html");

                var responceGetBytes = Encoding.UTF8.GetBytes("HTTP/1.0 200 OK" + Environment.NewLine +
                    //// "Location: https://softuni.bg" + Environment.NewLine + Environment.NewLine
                    //// "Content-Type: text/html" + Environment.NewLine +
                    //// "Content-Disposition: attachment; filename=index.exe" + Environment.NewLine +
                    //// "Content-Type: text/plain" + Environment.NewLine +
                    "Content-Length: " + responseText.Length + Environment.NewLine + Environment.NewLine +
                    responseText);

                stream.Write(responceGetBytes, 0, responceGetBytes.Length);
            }
        }

        public void Stop()
        {
            this.isRunning = false;
        }
    }
}