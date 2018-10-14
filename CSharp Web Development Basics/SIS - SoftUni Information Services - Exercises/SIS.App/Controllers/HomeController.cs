using SIS.Framework.ActionResult.Contracts;
using SIS.Framework.Controlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.App.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}