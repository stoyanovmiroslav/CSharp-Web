using System;
using System.Net;

namespace _01.URL_Decode
{
    class StartUp
    {
        static void Main(string[] args)
        {
            string encodeUrlString = Console.ReadLine();
            string decodeUrl = DecodeUrlString(encodeUrlString);

            Console.WriteLine(decodeUrl);

        }

        private static string DecodeUrlString(string emcodeUrl)
        {
            return WebUtility.UrlDecode(emcodeUrl);
        }
    }
}
