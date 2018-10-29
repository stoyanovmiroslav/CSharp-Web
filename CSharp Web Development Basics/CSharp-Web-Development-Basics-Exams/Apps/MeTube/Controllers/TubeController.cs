using MeTube.Models;
using MeTube.ViewModels.Tube;
using SIS.HTTP.Responses;
using SIS.MvcFramework;
using System;
using System.Linq;

namespace MeTube.Controllers
{
    public class TubeController : BaseController
    {
        public IHttpResponse Details(TubeViewModel model)
        {
            if (this.User.Username == null)
            {
                return Redirect("/Account/Login");
            }

            var tube = Db.Tubes.FirstOrDefault(x => x.Id == model.Id);

            if (tube == null)
            {
                return this.BadRequestError("Invalid Tube!");
            }

            tube.Views += 1;
            Db.SaveChanges();


            return this.View(tube);
        }

        public IHttpResponse Upload()
        {
            if (this.User.Username == null)
            {
                return Redirect("/Account/Login");
            }

            return this.View();
        }

        [HttpPost]
        public IHttpResponse Upload(TubeViewModel model)
        {
            var user = Db.Users.FirstOrDefault(x => x.Username == this.User.Username);

            if (user == null)
            {
                return Redirect("/Account/Login");
            }

            var urlParts = model.YoutubeLink.Split("?");

            //todo
            var urlData = urlParts[1].Split(new string[] { "=", "&" }, StringSplitOptions.RemoveEmptyEntries);
            
            model.YoutubeId = urlData[1];

            var tube = model.To<Tube>();
            tube.User = user;

            Db.Tubes.Add(tube);
            Db.SaveChanges();

            return this.Redirect("/");
        }
    }
}