﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.Framework.Utilities
{
    public class ControllerUtilities
    {
        public static string GetContorllerName(object controller) => 
            controller.GetType()
                      .Name
                      .Replace(MvcContext.Get.ControllerSuffix, string.Empty);

        public static string GetFullQualifiedName(string controller, string action) =>
            $"{MvcContext.Get.ViewFolderFullPath}/{controller}/{action}.{MvcContext.Get.HtmlFileExtention}";
    }
}