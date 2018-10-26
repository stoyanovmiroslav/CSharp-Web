using System.Linq;
using IRunes.Models;
using IRunes.ViewModels.Track;
using Microsoft.EntityFrameworkCore;
using SIS.HTTP.Extensions;
using SIS.HTTP.Responses.Contracts;
using SIS.MvcFramework;
using SIS.MvcFramework.HttpAttributes;

namespace IRunes.Controlers
{
    public class TrackController : BaseController
    {
        [HttpGet("/track/create")]
        public IHttpResponse Create(CreateTrackViewModel model)
        {
            if (!this.User.Exist)
            {
                return this.BadRequestError("To create an album you have to login first!", "/user/login");
            }

            return this.View(model);
        }

        [HttpPost("/track/create")]
        public IHttpResponse Create(CreateTrackActionViewModel model)
        {
            if (!this.User.Exist)
            {
                return this.BadRequestError("To create an album you have to login first!", "/user/login");
            }

            if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.Link) || model.Price == 0)
            {
                return this.BadRequestError("Invalid data, please try again!", "/track/create", model);
            }

            var album = this.db.Albums.Include(x => x.Tracks).FirstOrDefault(x => x.Id == model.AlbumId);

            if (album == null)
            {
                return this.BadRequestError("Invalid album, please try again!", "/track/create", model);
            }

            model.Link = model.Link.Replace("youtube.com/watch?v=",
                                                 "youtube.com/embed/");

            var track = model.To<Track>();

            album.Tracks.Add(track);
            db.SaveChanges();

            return this.View(album, "/album/details");
        }

        [HttpGet("/track/details")]
        public IHttpResponse Details(DetailsTrackViewModel model)
        {
            if (!this.User.Exist)
            {
                return this.BadRequestError("To see track details you have to login first!", "/user/login");
            }

            var track = this.db.Tracks.FirstOrDefault(x => x.Id == model.TrackId);

            if (track == null)
            {
                return this.BadRequestError("Invalid data, please try again!", "/track/create");
            }

            model.TrackLink = track.Link.UrlDecode();
            model.TrackName = track.Name;
            model.TrackPrice = track.Price;

            return this.View(model);
        }
    }
}