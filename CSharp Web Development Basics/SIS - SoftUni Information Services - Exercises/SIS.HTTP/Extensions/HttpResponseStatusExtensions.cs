using SIS.HTTP.Enums;
using SIS.HTTP.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Extensions
{
    public static class HttpResponseStatusExtensions
    {
        public static string GetResponseLine(this HttpResponseStatusCode statusCode)
        {
            string responseLine = $"{(int)statusCode} {statusCode}";

            return responseLine;
        }
    }
}