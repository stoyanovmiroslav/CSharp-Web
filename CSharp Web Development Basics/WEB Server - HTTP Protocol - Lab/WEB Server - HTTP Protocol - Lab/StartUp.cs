using System;

namespace WEB_Server___HTTP_Protocol___Lab
{
    class StartUp
    {
        static void Main(string[] args)
        {
            IHttpServer httpServer = new HttpServer();
            httpServer.Start();
        }
    }
}
