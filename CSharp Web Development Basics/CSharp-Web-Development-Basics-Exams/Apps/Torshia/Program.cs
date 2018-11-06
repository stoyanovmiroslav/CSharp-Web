using SIS.MvcFramework;

namespace MeTube
{
    class Program
    {
        static void Main(string[] args)
        {
            WebHost.Start(new Startup());
        }
    }
}