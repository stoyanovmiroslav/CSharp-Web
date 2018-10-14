using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.Framework.Utilities
{
    public class ControllerUtilities
    {
        public static string GetContorlerName(object controller) => 
            controller.GetType()
                      .Name
                      .Replace(MvcContext.Get.ControllerSuffix, string.Empty);

        public static string GetFullQualifiedName(string controller, string action) =>
            $"../../../{MvcContext.Get.ViewFolder}/{controller}/{action}.html";


    }
}