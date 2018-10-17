using System.Linq;
using System.Text;
using System.Web;
using IRunes.Models;
using IRunes.ViewModels.Album;
using IRunes.ViewModels.Home;
using Microsoft.EntityFrameworkCore;
using SIS.HTTP.Responses.Contracts;
using SIS.MvcFramework.HttpAttributes;

namespace IRunes.Controlers
{
    public class AlbumController : BaseController
    {
        [HttpGet("/album/create")]
        public IHttpResponse Create()
        {
            if (this.User == null)
            {
                return this.Redirect("/user/login");
            }

            return this.View();
        }

        [HttpPost("/album/create")]
        public IHttpResponse Create(CreateAlbumViewModel model)
        {
            if (this.User == null)
            {
                return this.Redirect("/user/login");
            }

            if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.Cover))
            {
                return this.Redirect("/album/create");
            }

            var user = db.Users.FirstOrDefault(x => x.Username == this.User);

            if (user == null)
            {
                return this.Redirect("/user/login");
            }

            user.Albums.Add(new Album { Name = model.Name, Cover = model.Cover });
            db.SaveChanges();

            return this.Redirect("/album/all");
        }

        [HttpGet("/album/details")]
        public IHttpResponse Details(AlbumDetailsViewModel model)
        {
            if (this.User == null)
            {
                return this.Redirect("/user/login");
            }

            var album = this.db.Albums.Include(x => x.Tracks).FirstOrDefault(x => x.Id == model.AlbumId);
            string albumCover = HttpUtility.UrlDecode(album.Cover);

            var tracksPrice = album.Tracks.Sum(x => x.Price);
            var tracksPriceAfterDiscount = tracksPrice - (tracksPrice * 13 / 100);

            model.Tracks = album.Tracks.ToList();
            model.AlbumName = album.Name;
            model.AlbumCover = album.Cover;
            model.TracksPriceAfterDiscount = tracksPriceAfterDiscount;

            return this.View(model);
        }

        [HttpGet("/album/all")]
        public IHttpResponse All()
        {
            if (this.User == null)
            {
                return this.Redirect("/user/login");
            }

            var user = db.Users.Include(x => x.Albums).FirstOrDefault(x => x.Username == this.User);

            if (user == null)
            {
                return this.Redirect("/user/login");
            }

            var albums = user.Albums.ToList();
            var model = new AllAlbumsViewModel { Albums = albums };

            return this.View(model);
        }
    }
}