using SIS.HTTP.Enums;

namespace SIS.MvcFramework.HttpAttributes
{
    public class HttpGetAttribute : HttpAttribute
    {
        public HttpGetAttribute(string path)
            : base(path)
        {
        }

        public override HttpRequestMethod Method => HttpRequestMethod.GET;
    }
}