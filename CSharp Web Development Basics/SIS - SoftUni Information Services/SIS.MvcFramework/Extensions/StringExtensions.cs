using System.Net;

namespace SIS.HTTP.Extensions
{
    public static class StringExtensions
    {
        public static string UrlDecode(this string input)
        {
            return WebUtility.UrlDecode(input);
        }
    }
}