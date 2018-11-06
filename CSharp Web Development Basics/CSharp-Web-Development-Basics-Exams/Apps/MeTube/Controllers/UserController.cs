using MeTube.ViewModels.User;
using Microsoft.EntityFrameworkCore;
using SIS.HTTP.Responses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeTube.Controllers
{
    public class UserController : BaseController
    {
        public IHttpResponse Profile()
        {
            var user = Db.Users.Include(x => x.Tubes).FirstOrDefault(x => x.Username == this.User.Username);

            if (user == null)
            {
                return this.BadRequestError("Invalid user!");
            }

            var model = new ProfileUserViewModel
            {
                Username = user.Username,
                Email = user.Email.Replace("@", "&commat;"),
                Tubes = user.Tubes.ToList()
            };


            return this.View(model);
        }
    }
}
