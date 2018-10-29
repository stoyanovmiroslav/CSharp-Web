using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SIS.Framework.Views
{
    public class ViewEngine
    {
        private const string DisplayTemplateSuffix = "DisplayTemplate";

        private const string DisplayTemplatesFolderName = "DisplayTemplates";

        private const string LayoutViewName = "_Layout";

        private const string ErrorViewName = "_Error";

        private const string ViewExtension = "html";

        private const string ModelCollectionViewParameterPattern = @"\@Model\.Collection\.(\w+)\((.+)\)";

        private string ViewsFolderPath =>
            $@"{MvcContext.Get.RootDirectoryRelativePath}/{MvcContext.Get.ViewFolderFullPath}";

        private string ViewsSharedFolderPath => 
            $@"{ViewsFolderPath}/{MvcContext.Get.SharedViewsFolderName}";

        private string ViewsDisplayTemplateFolderPath =>
            $@"{ViewsSharedFolderPath}/{DisplayTemplatesFolderName}";

        private string FormatLayoutViewPath() =>
            $@"{ViewsSharedFolderPath}/{MvcContext.Get.LayoutViewName}.{ViewExtension}";

        private string FormatErrorViewPath =>
            $@"{ViewsSharedFolderPath}/{ErrorViewName}.{ViewExtension}";

        private string FormatViewPath(string controllerName, string actionName) =>
            $@"{ViewsFolderPath}/{controllerName}/{actionName}.{ViewExtension}";

        private string FormatDisplayTemplatePath(string objectName)
            => $@"{ViewsDisplayTemplateFolderPath}/{objectName}{DisplayTemplateSuffix}.{ViewExtension}";

        private string ReadFileHtml(string viewPath)
        {
            if (!File.Exists(viewPath))
            {
                throw new FileNotFoundException($"This view => {viewPath} could not be found");
            }

            return File.ReadAllText(viewPath);
        }

        public string GetErrorContent(string viewBodyPath = null)
        {
            var errorContent = this.ReadFileHtml(this.FormatErrorViewPath);

            if (viewBodyPath != null)
            {
                var bodyContent = ReadFileHtml($"{ViewsFolderPath}/{viewBodyPath}.{ViewExtension}");
                errorContent = string.Concat(errorContent, bodyContent);
            }

            var layoutContent = this.ReadFileHtml(this.FormatLayoutViewPath());

            return layoutContent.Replace("@RenderBody()", errorContent);
        }

        public string GetViewContent(string controller, string action)
        {
            var layoutContent = this.ReadFileHtml(this.FormatLayoutViewPath());
            var bodyContent = this.ReadFileHtml(this.FormatViewPath(controller, action));

            return layoutContent.Replace("@RenderBody()", bodyContent);
        }

        public string RenderHtml(string fullHtmlContent, IDictionary<string, object> viewData)
        {
            string renderedHtml = fullHtmlContent;

            if (viewData.Count > 0)
            {
                foreach (var parameter in viewData)
                {
                    renderedHtml =
                        this.RenderViewData(renderedHtml, parameter.Value, parameter.Key);
                }

                if (viewData.ContainsKey("Error"))
                {
                    renderedHtml = renderedHtml.Replace(@"@Error", viewData["error"].ToString());
                }
            }

            return renderedHtml;
        }

        private string RenderViewData(string template, object viewObject, string viewObjectName = null)
        {
            if (viewObject != null && viewObject.GetType() != typeof(string) &&
               viewObject is IEnumerable collectionProperty && Regex.IsMatch(template, ModelCollectionViewParameterPattern))
            {
                Match collectionMatch = Regex.Matches(template, ModelCollectionViewParameterPattern)
                    .First(m => m.Groups[1].Value == viewObjectName);

                var fullMatch = collectionMatch.Groups[0].Value;
                var itemPattern = collectionMatch.Groups[2].Value;

                string result = string.Empty;

                foreach (var element in collectionProperty)
                {
                    result += itemPattern.Replace("@Item", this.RenderViewData(template, element));
                }

                return template.Replace(fullMatch, result);
            }

            if (viewObject != null && !viewObject.GetType().IsPrimitive &&
               viewObject.GetType() != typeof(string))
            {
                var objectDisplayTemplate = this.FormatDisplayTemplatePath(viewObject.GetType().Name);
                if (File.Exists(objectDisplayTemplate))
                {
                    string renderedObject = this.RenderObject(viewObject,
                        File.ReadAllText(objectDisplayTemplate));

                    return viewObjectName != null
                        ? template.Replace($"@Model.{viewObjectName}", renderedObject)
                        : renderedObject;
                }
            }

            return viewObjectName != null
                ? template.Replace($"@Model.{viewObjectName}", viewObject?.ToString())
                : viewObject?.ToString();
        }

        private string RenderObject(object viewObject, string displayTemplate)
        {
            var viewObjectType = viewObject.GetType();
            var viewObjectProperties = viewObjectType.GetProperties();

            foreach (var viewObjectProperty in viewObjectProperties)
            {
                displayTemplate = this.RenderViewData(
                    displayTemplate,
                    viewObjectProperty.GetValue(viewObject),
                    viewObjectProperty.Name);
            }

            return displayTemplate;
        }
    }
}