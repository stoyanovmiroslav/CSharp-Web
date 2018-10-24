using System.Reflection;

namespace SIS.Framework
{
    public class MvcContext
    {
        private static MvcContext Instance;

        public static MvcContext Get => Instance == null ? Instance = new MvcContext() : Instance;

        public string AssemblyName => Assembly.GetEntryAssembly().GetName().Name;

        public string ControllersFolder => "Controllers";

        public string ControllerSuffix => "Controller";

        public string ViewFolderFullPath => "../../../Views";

        public string ErrorViewFolder => "Errors";

        public string ModelsFolder => "Models";

        public string HtmlFileExtention => "html";
    }
}