using SIS.HTTP.Enums;
using SIS.HTTP.Headers;
using SIS.HTTP.Responses;

namespace SIS.WebServer.Results
{
    public class InlineResourseResult : HttpResponse
    {
        public InlineResourseResult(HttpResponseStatusCode statusCode, byte[] content)
            : base(statusCode)
        {
            this.Headers.Add(new HttpHeader(HttpHeader.CONTENT_LENGTH, content.Length.ToString()));
            this.Headers.Add(new HttpHeader(HttpHeader.CONTENT_DISPOSITION, "inline"));

            this.Content = content;
        }
    }
}