using SIS.WebServer;
using System;
using System.Reflection;

namespace SIS.Framework
{
    public class MvcEngine
    {
        public static void Run(Server server)
        {
            RegisterAsemblyName();
            RegisterControllersData();
            RegisterViewsData();
            RegisterModelsData();
            RegisterErrorViewFolder();

            try
            {
                server.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void RegisterErrorViewFolder()
        {
            MvcContext.Get.ErrorViewFolder = "Error";
        }

        private static void RegisterModelsData()
        {
            MvcContext.Get.ModelsFolder = "Models";
        }

        private static void RegisterViewsData()
        {
            MvcContext.Get.ViewFolderFullPath = "../../../Views";
        }

        private static void RegisterAsemblyName()
        {
            MvcContext.Get.AssemblyName = Assembly.GetEntryAssembly().GetName().Name;
        }

        private static void RegisterControllersData()
        {
            MvcContext.Get.ControllersFolder = "Controllers";
            MvcContext.Get.ControllerSuffix = "Controller";
        }
    }
}