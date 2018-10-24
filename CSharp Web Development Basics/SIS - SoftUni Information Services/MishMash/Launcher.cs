using SIS.MvcFramework;

namespace MishMash
{
    public class Launcher
    {
        static void Main(string[] args)
        {
            WebHost.Start(new StartUp());   
        }
    }
}