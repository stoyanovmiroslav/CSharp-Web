using System;
using System.Net;

namespace _02.Validate_URL
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            string encodeUrlString = Console.ReadLine();
            string decodedUrlString = WebUtility.UrlDecode(encodeUrlString);

            var urlParts = new Uri(decodedUrlString);

            string protocol = urlParts.Scheme;
            string host = urlParts.Host;
            int? port = urlParts.Port;
            string path = urlParts.AbsolutePath;
            string queryString = urlParts.Query;
            string fragment = urlParts.Fragment;

            if (protocol == null || host == null || port == null || path == null ||
                (protocol == "http" && port != 80) || (protocol == "https" && port != 443))
            {
                Console.WriteLine("Invalid URL");
                Environment.Exit(0);
            }

            Console.WriteLine($"Protocol: {protocol}");
            Console.WriteLine($"Host: {host}");
            Console.WriteLine($"Port: {port}");
            Console.WriteLine($"Path: {path}");

            if (string.IsNullOrWhiteSpace(queryString))
            {
                Console.WriteLine($"Query: {queryString}");
            }

            if (string.IsNullOrWhiteSpace(queryString))
            {
                Console.WriteLine($"Fragment: {fragment}");
            }
        }
    }
}