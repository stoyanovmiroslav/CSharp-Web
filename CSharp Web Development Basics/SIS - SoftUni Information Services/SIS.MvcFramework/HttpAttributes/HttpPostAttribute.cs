using SIS.HTTP.Enums;

namespace SIS.MvcFramework.HttpAttributes
{
    public class HttpPostAttribute : HttpAttribute
    {
        public HttpPostAttribute(string path) 
            : base(path)
        {
        }

        public override HttpRequestMethod Method => HttpRequestMethod.POST;
    }
}