﻿using System;

namespace SIS.Framework
{
    public class MvcContext
    {
        private static MvcContext Instance;

        public static MvcContext Get => Instance == null ? Instance = new MvcContext() : Instance;

        public string AssemblyName { get; set; }

        public string ControllersFolder { get; set; }

        public string ControllerSuffix { get; set; }

        public string ViewFolderFullPath { get; set; }

        public string ErrorViewFolder { get; set; }

        public string ModelsFolder { get; set; }

        public string HtmlFileExtention { get; set; } = "html";
    }
}