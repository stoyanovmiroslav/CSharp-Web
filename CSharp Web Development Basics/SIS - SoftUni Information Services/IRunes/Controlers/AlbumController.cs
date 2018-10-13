using System.Linq;
using System.Text;
using System.Web;
using IRunes.Models;
using IRunes.ViewModels.Album;
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
        public IHttpResponse DoCreate(DoCreateViewModel model)
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
        public IHttpResponse Details(DetailsViewModel model)
        {
            if (this.User == null)
            {
                return this.Redirect("/user/login");
            }

            var album = this.db.Albums.Include(x => x.Tracks).FirstOrDefault(x => x.Id == model.AlbumId);
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

            return this.View();
        }

        [HttpGet("/album/all")]
        public IHttpResponse All()
        {
            if (this.User == null)
            {
                return this.Redirect("/user/login");
            }

            this.ViewBag["albums"] = "There are currently no albums.";

            var user = db.Users.Include(x => x.Albums).FirstOrDefault(x => x.Username == this.User);

            if (user == null)
            {
                return this.Redirect("/user/login");
            }

            var albums = user.Albums.ToArray();

            string albumListHtml = System.IO.File.ReadAllText(VIEWS_FOLDER_PATH + "/Parts/AlbumList" + HTML_EXTENTION);

            var sb = new StringBuilder();

            foreach (var album in albums)
            {
                sb.Append(albumListHtml.Replace("{{albumId}}", album.Id)
                                       .Replace("{{albumName}}", album.Name));
            }

            if (sb.Length != 0)
            {
                this.ViewBag["albums"] = sb.ToString();
            }

            return this.View();
        }
    }
}