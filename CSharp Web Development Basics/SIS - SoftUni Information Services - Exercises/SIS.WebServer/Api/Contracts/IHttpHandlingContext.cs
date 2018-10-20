using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;

namespace SIS.WebServer.Api.Contracts
{
    public interface IHttpHandlingContext
    {
        IHttpResponse Handle(IHttpRequest request);
    }
}