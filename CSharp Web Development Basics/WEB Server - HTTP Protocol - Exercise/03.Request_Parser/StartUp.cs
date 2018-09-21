using System;
using System.Collections.Generic;
using System.Net;

namespace _03.Request_Parser
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            string input;
            var routesByHtppMethods = new Dictionary<string, HashSet<string>>();

            while ((input = Console.ReadLine().ToLower()) != "end")
            {
                string[] inputArgs = input.Split("/", StringSplitOptions.RemoveEmptyEntries);

                string path = inputArgs[0];
                string method = inputArgs[1];

                if (!routesByHtppMethods.ContainsKey(method))
                {
                    routesByHtppMethods.Add(method, new HashSet<string>());
                }

                routesByHtppMethods[method].Add(path);
            }

            string[] htppRequest = Console.ReadLine().ToLower().Split(" ", StringSplitOptions.RemoveEmptyEntries);

            string requestedMethod = htppRequest[0];
            string requestedPath = htppRequest[1].Trim('/');

            HttpStatusCode httpStatusCode = HttpStatusCode.OK;

            if (!routesByHtppMethods.ContainsKey(requestedMethod) || !routesByHtppMethods[requestedMethod].Contains(requestedPath))
            {
                httpStatusCode = HttpStatusCode.NotFound;
            }

            Console.WriteLine($"HTTP/1.1 {(int)httpStatusCode} {httpStatusCode}");
            Console.WriteLine($"Content-Length: {httpStatusCode.ToString().Length}");
            Console.WriteLine($"Content-Type: text/plain");
            Console.WriteLine();
            Console.WriteLine($"{httpStatusCode}");
        }
    }
}