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
    public class TrackController : BaseController
    {
        [HttpGet("/track/create")]
        public IHttpResponse Create()
        {
            if (this.User == null)
            {
                return this.Redirect("/user/login");
            }

            string albumId = this.Request.QueryData["albumId"].ToString();
            this.ViewBag["albumId"] = albumId;

            return this.View();
        }

        [HttpPost("/track/create")]
        public IHttpResponse CreatePost()
        {
            if (this.User == null)
            {
                return this.Redirect("/user/login");
            }

            string albumId = this.Request.QueryData["albumId"].ToString();

            string name = this.Request.FormData["name"].ToString();
            string link = this.Request.FormData["link"].ToString();
            decimal price = decimal.Parse(this.Request.FormData["price"].ToString());

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(link) || price == 0)
            {
                return this.Redirect("/track/create");
            }

            var album = this.db.Albums.Include(x => x.Tracks).FirstOrDefault(x => x.Id == albumId);

            album.Tracks.Add(new Track { Name = name, Link = link, Price = price });
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

            return this.View("/album/details");
        }

        [HttpGet("/track/details")]
        public IHttpResponse Details()
        {
            if (this.User == null)
            {
                return this.Redirect("/user/login");
            }

            var trackId = this.Request.QueryData["id"].ToString();
            var albumId = this.Request.QueryData["albumId"].ToString();

            var track = this.db.Tracks.FirstOrDefault(x => x.Id == trackId);

            string trackLink = HttpUtility.UrlDecode(track.Link);

            this.ViewBag["albumId"] = albumId;

            this.ViewBag["trackLink"] = trackLink;
            this.ViewBag["trackVideo"] = this.InsertViewParameters("/Parts/Video");

            this.ViewBag["trackName"] = track.Name;
            this.ViewBag["trackPrice"] = track.Price.ToString();
            this.ViewBag["trackInfo"] = this.InsertViewParameters("/Parts/TrackInfo");

            return this.View();
        }
    }
}