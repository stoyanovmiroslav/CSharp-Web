using System.Linq;
using System.Text;
using System.Web;
using IRunes.Models;
using IRunes.ViewModels.Track;
using Microsoft.EntityFrameworkCore;
using SIS.HTTP.Responses.Contracts;
using SIS.MvcFramework.HttpAttributes;

namespace IRunes.Controlers
{
    public class TrackController : BaseController
    {
        [HttpGet("/track/create")]
        public IHttpResponse Create(CreateTrackViewModel model)
        {
            if (this.User == null)
            {
                return this.Redirect("/user/login");
            }

            this.ViewBag["albumId"] = model.AlbumId;

            return this.View();
        }

        [HttpPost("/track/create")]
        public IHttpResponse DoCreate(DoCreateTrackViewModel model)
        {
            if (this.User == null)
            {
                return this.Redirect("/user/login");
            }

            if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.Link) || model.Price == 0)
            {
                return this.Redirect("/track/create");
            }

            var album = this.db.Albums.Include(x => x.Tracks).FirstOrDefault(x => x.Id == model.AlbumId);

            album.Tracks.Add(new Track { Name = model.Name, Link = model.Link, Price = model.Price });
            db.SaveChanges();

            string albumCover = HttpUtility.UrlDecode(album.Cover);

            var tracksPrice = album.Tracks.Sum(x => x.Price);
            var tracksPriceAfterDiscount = tracksPrice - (tracksPrice * 13 / 100);

            string albumData = System.IO.File.ReadAllText(VIEWS_FOLDER_PATH + "/Parts/AlbumInfo" + HTML_EXTENTION);

            albumData = albumData.Replace("{{tracksPriceAfterDiscount}}", tracksPriceAfterDiscount.ToString())
                                 .Replace("{{albumName}}", album.Name)
                                 .Replace("{{albumCover}}", albumCover);

            var tracks = album.Tracks.ToArray();

            var sbTracks = new StringBuilder();

            this.ViewBag["tracks"] = "";

            if (tracks.Length > 0)
            {
                string trackList = System.IO.File.ReadAllText(VIEWS_FOLDER_PATH + "/Parts/TrackList" + HTML_EXTENTION);

                for (int i = 1; i <= tracks.Length; i++)
                {
                    var track = tracks[i - 1];

                    string replacedTrackList = trackList.Replace("{{numeration}}", i.ToString())
                                                        .Replace("{{trackId}}", track.Id)
                                                        .Replace("{{albumId}}", model.AlbumId)
                                                        .Replace("{{trackName}}", track.Name);

                    sbTracks.Append(replacedTrackList);
                }

                this.ViewBag["tracks"] = sbTracks.ToString();
            }

            this.ViewBag["albumId"] = album.Id;
            this.ViewBag["album"] = albumData.ToString();

            return this.View("/album/details");
        }

        [HttpGet("/track/details")]
        public IHttpResponse Details(DetailsTrackViewModel model)
        {
            if (this.User == null)
            {
                return this.Redirect("/user/login");
            }

            var track = this.db.Tracks.FirstOrDefault(x => x.Id == model.TrackId);

            string trackLink = HttpUtility.UrlDecode(track.Link);

            this.ViewBag["albumId"] = model.AlbumId;

            this.ViewBag["trackLink"] = trackLink;
            this.ViewBag["trackVideo"] = this.InsertViewParameters("/Parts/Video");

            this.ViewBag["trackName"] = track.Name;
            this.ViewBag["trackPrice"] = track.Price.ToString();
            this.ViewBag["trackInfo"] = this.InsertViewParameters("/Parts/TrackInfo");

            return this.View();
        }
    }
}