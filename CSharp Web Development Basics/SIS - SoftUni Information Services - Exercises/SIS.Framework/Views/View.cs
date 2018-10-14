using SIS.Framework.ActionResult.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SIS.Framework.Views
{
    public class View : IRenderable
    {
        private readonly string fullyQualifiedTemplateName;

        public View(string fullyQualifiedTemplateName)
        {
            this.fullyQualifiedTemplateName = fullyQualifiedTemplateName;
        }

        public View(string fullyQualifiedTemplateName, Dictionary<string, string> viewBag) 
            : this(fullyQualifiedTemplateName)
        {
            this.ViewBag = viewBag;
        }

        public Dictionary<string, string> ViewBag { get; set; }

        private string ReadFile(string fullyQualifiedTemplateName)
        {
            if (!File.Exists(fullyQualifiedTemplateName))
            {
                throw new FileNotFoundException();
            }

            return File.ReadAllText(fullyQualifiedTemplateName);
        }

        public string Render()
        {
            var fullHtml = ReadFile(this.fullyQualifiedTemplateName);

            return InsertViewParameters(fullHtml);
        }

        protected string InsertViewParameters(string fileContent)
        {
            foreach (var viewBagKey in ViewBag.Keys)
            {
                string placeHolder = $"{{{{{viewBagKey}}}}}";

                if (fileContent.Contains(viewBagKey))
                {
                    fileContent = fileContent.Replace(placeHolder, this.ViewBag[viewBagKey]);
                }
            }

            return fileContent;
        }
    }
}