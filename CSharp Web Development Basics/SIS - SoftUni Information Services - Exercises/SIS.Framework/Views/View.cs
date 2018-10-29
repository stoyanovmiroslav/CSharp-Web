using SIS.Framework.ActionResult.Contracts;
using System.Collections.Generic;
using System.IO;

namespace SIS.Framework.Views
{
    public class View : IRenderable
    {
        private const string RenderBodyConstant = "@RenderBody()";

        private readonly string fullHtmlContent;

        public View(string fullHtmlContent)
        {
            this.fullHtmlContent = fullHtmlContent;
        }

        public string Render() => this.fullHtmlContent;


        //private readonly string fullyQualifiedTemplateName;

        //public View(string fullyQualifiedTemplateName)
        //{
        //    this.fullyQualifiedTemplateName = fullyQualifiedTemplateName;
        //}

        //public View(string fullyQualifiedTemplateName, IDictionary<string, object> viewData) 
        //    : this(fullyQualifiedTemplateName)
        //{
        //    this.ViewData = viewData;
        //}

        //private IDictionary<string, object> ViewData { get; set; }

        //private string ReadFile(string fullyQualifiedTemplateName)
        //{
        //    if (!File.Exists(fullyQualifiedTemplateName))
        //    {
        //        throw new FileNotFoundException();
        //    }

        //    return File.ReadAllText(fullyQualifiedTemplateName);
        //}

        //public string Render()
        //{
        //    var bodyHtml = ReadFile(this.fullyQualifiedTemplateName);
        //    this.ViewData["body"] = InsertViewParameters(bodyHtml);

        //    // TODO: Read layout path from const
        //    var layoutHtml = ReadFile("../../../Views/_Layout.html"); 

        //    return InsertViewParameters(layoutHtml);
        //}

        //protected string InsertViewParameters(string fileContent)
        //{
        //    foreach (var viewDataKey in this.ViewData.Keys)
        //    {
        //        string placeHolder = $"{{{{{viewDataKey}}}}}";

        //        if (fileContent.Contains(viewDataKey))
        //        {
        //            fileContent = fileContent.Replace(placeHolder, this.ViewData[viewDataKey].ToString());
        //        }
        //    }

        //    return fileContent;
        //}
    }
}