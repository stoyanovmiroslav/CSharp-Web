using SIS.Framework;

namespace CakeApp
{
    public class Launcher
    {
        static void Main(string[] args)
        {
            WebHost.Start(new StartUp());
        }
    }
}