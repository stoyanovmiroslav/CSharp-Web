using SIS.HTTP.Enums;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Api.Contracts;
using SIS.WebServer.Results;
using SIS.WebServer.Routing;
using System.IO;

namespace SIS.Framework
{
    public class HttpHandler : IHttpHandler
    {
        private const string RESOURCES_FOLDER_PATH = "../../../Resources";

        public ServerRoutingTable serverRoutingTable;

        public HttpHandler(ServerRoutingTable serverRoutingTable)
        {
            this.serverRoutingTable = serverRoutingTable;
        }

        public IHttpResponse HandlerRequest(IHttpRequest httpRequest)
        {
            if (!this.serverRoutingTable.Routes.ContainsKey(httpRequest.RequestMethod) ||
               !this.serverRoutingTable.Routes[httpRequest.RequestMethod].ContainsKey(httpRequest.Path))
            {
                return returnIfResource(httpRequest.Path);
            }

            return this.serverRoutingTable.Routes[httpRequest.RequestMethod][httpRequest.Path].Invoke(httpRequest);
        }


        private IHttpResponse returnIfResource(string path)
        {
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

            return new HttpResponse(HttpResponseStatusCode.NotFound);
        }
    }
}