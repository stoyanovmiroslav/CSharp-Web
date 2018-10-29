using MeTube.Data;
using SIS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeTube.Controllers
{
    public class BaseController : Controller
    {
        protected MeTubeDbContext Db;

        public BaseController()
        {
            this.Db = new MeTubeDbContext();
        }
    }
}