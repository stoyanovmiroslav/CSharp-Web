using MishMash.Data;
using SIS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MishMash.Controllers
{
    public class BaseController : Controller
    {
        protected MishMashDbContext db;

        public BaseController()
        {
            db = new MishMashDbContext();
        }
    }
}