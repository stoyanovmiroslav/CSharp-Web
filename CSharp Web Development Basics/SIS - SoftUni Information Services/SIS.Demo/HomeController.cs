using System;
using SIS.HTTP.Enums;
using SIS.HTTP.Responses.Contracts;
using SIS.MvcFramework;

namespace SIS.Demo
{
    public class HomeController : Controller
    {
        public IHttpResponse Index()
        {
            return this.View();
        }
    }
}