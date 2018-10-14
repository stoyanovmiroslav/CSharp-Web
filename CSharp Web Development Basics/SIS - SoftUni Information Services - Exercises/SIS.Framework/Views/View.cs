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
            return fullHtml;
        }
    }
}