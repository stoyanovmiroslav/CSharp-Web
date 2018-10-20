using SIS.HTTP.Common;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Api.Contracts;
using SIS.WebServer.Results;
using System.IO;
using System.Linq;

namespace SIS.Framework.Routers
{
    public class ResourceRouter : IHttpHandler
    {
        private const string RESOURCES_FOLDER_PATH = "../../../Resources";

        public IHttpResponse HandlerRequest(IHttpRequest httpRequest)
        {
            var path = httpRequest.Path;

            int indexOfLastDot = path.LastIndexOf('.');
            int indexOflastSlash = path.LastIndexOf('/');

            string fileFolder = path.Substring(indexOfLastDot + 1);
            string fileFullName = path.Substring(indexOflastSlash + 1);

            string resourceFullPath = $"{RESOURCES_FOLDER_PATH}/{fileFolder}/{fileFullName}";

            if (File.Exists(resourceFullPath))
            {
                byte[] resource = File.ReadAllBytes(resourceFullPath);

                return new InlineResourseResult(HttpResponseStatusCode.Ok, resource);
            }

            return new TextResult(HttpResponseStatusCode.NotFound, $"File {fileFullName} Not Found!");
        }
    }
}