using System.Linq;
using IRunes.Models;
using IRunes.ViewModels.Album;
using Microsoft.EntityFrameworkCore;
using SIS.HTTP.Responses.Contracts;
using SIS.MvcFramework;
using SIS.MvcFramework.HttpAttributes;

namespace IRunes.Controlers
{
    public class AlbumController : BaseController
    {
        [HttpGet("/album/create")]
        public IHttpResponse Create()
        {
            if (!this.User.Exist)
            {
                return this.BadRequestError("To create an album you have to login first!", "/user/login");
            }

            return this.View();
        }

        [HttpPost("/album/create")]
        public IHttpResponse Create(CreateAlbumViewModel model)
        {
            if (!this.User.Exist)
            {
                return this.BadRequestError("To create an album you have to login first!", "/user/login");
            }

            if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.Cover))
            {
                return this.BadRequestError("Invalid data!", "/album/create");
            }

            var user = db.Users.FirstOrDefault(x => x.Username == this.User.Name);

            if (user == null)
            {
                return this.BadRequestError("Invalid Operation, you need to login again!", "/user/login");
            }

            var album = model.To<Album>();

            user.Albums.Add(album);
            db.SaveChanges();

            return this.Redirect("/album/all");
        }

        [HttpGet("/album/details")]
        public IHttpResponse Details(AlbumDetailsViewModel model)
        {
            if (!this.User.Exist)
            {
                return this.BadRequestError("To see your album details you have to login first!", "/user/login");
            }

            var album = this.db.Albums.Include(x => x.Tracks).FirstOrDefault(x => x.Id == model.AlbumId);

            if (album == null)
            {
                return this.BadRequestError("To see your album details you have to login first!", "/user/login", model);
            }

            return this.View(album);
        }

        [HttpGet("/album/all")]
        public IHttpResponse All()
        {
            if (!this.User.Exist)
            {
                return this.BadRequestError("To see your Аlbums you have to login first!", "/user/login");
            }

            var user = db.Users.Include(x => x.Albums).FirstOrDefault(x => x.Username == this.User.Name);

            if (user == null)
            {
                return this.BadRequestError("Invalid Operation, you need to login again!", "/user/login");
            }

            return this.View(user.Albums.ToArray());
        }
    }
}