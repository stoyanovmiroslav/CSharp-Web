using SIS.MvcFramework;

namespace IRunes
{
    public class Launcher
    {
        static void Main(string[] args)
        {
            WebHost.Start(new StartUp());   
        }
    }
}