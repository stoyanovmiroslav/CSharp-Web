using SIS.MvcFramework;

namespace FluffyCats
{
    class Program
    {
        static void Main(string[] args)
        {
            WebHost.Start(new Startup());
        }
    }
}