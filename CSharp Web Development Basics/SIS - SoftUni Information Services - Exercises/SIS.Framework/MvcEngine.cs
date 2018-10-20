using SIS.WebServer;
using System;

namespace SIS.Framework
{
    public class MvcEngine
    {
        public static void Run(Server server)
        {
            try
            {
                server.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}