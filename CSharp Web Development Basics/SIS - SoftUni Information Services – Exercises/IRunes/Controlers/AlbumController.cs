using System;
using System.Linq;
using System.Text;
using System.Web;
using IRunes.Models;
using Microsoft.EntityFrameworkCore;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.MvcFramework.HttpAttributes;
using SIS.WebServer.Results;

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
        public IHttpResponse CreatePost()
        {
            if (this.User == null)
            {
                return this.Redirect("/user/login");
            }

            string name = this.Request.FormData["name"].ToString();
            string cover = this.Request.FormData["cover"].ToString();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(cover))
            {
                return this.Redirect("/album/create");
            }

            var user = db.Users.FirstOrDefault(x => x.Username == this.User);

            if (user == null)
            {
                return this.Redirect("/user/login");
            }

            user.Albums.Add(new Album { Name = name, Cover = cover });
            db.SaveChanges();

            return this.Redirect("/album/all");
        }

        [HttpGet("/album/details")]
        public IHttpResponse Details()
        {
            if (this.User == null)
            {
                return this.Redirect("/user/login");
            }

            var albumId = this.Request.QueryData["id"].ToString();

            var album = this.db.Albums.Include(x => x.Tracks).FirstOrDefault(x => x.Id == albumId);
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

                for (int i = 1; i < tracks.Length; i++)
                {
                    var track = tracks[i];

                    string replacedTrackList = trackList.Replace("{{numeration}}", i.ToString())
                                                        .Replace("{{trackId}}", track.Id)
                                                        .Replace("{{albumId}}", albumId)
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