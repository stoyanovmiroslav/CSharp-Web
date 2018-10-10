﻿using System;
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
                return this.View("User/Login");
            }

            return this.View();
        }

        [HttpPost("/album/create")]
        public IHttpResponse CreatePost()
        {
            if (this.User == null)
            {
                return this.View("User/Login");
            }

            string name = this.Request.FormData["name"].ToString();
            string cover = this.Request.FormData["cover"].ToString();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(cover))
            {
                return this.View("/album/create");
            }

            var user = db.Users.FirstOrDefault(x => x.Username == this.User);

            if (user == null)
            {
                return this.View("User/Login");
            }

            user.Albums.Add(new Album { Name = name, Cover = cover });
            db.SaveChanges();

            var response = All();
            return response;
        }

        [HttpGet("/album/details")]
        public IHttpResponse Details()
        {
            if (this.User == null)
            {
                return this.View("User/Login");
            }

            var albumId = this.Request.QueryData["id"].ToString();

            var album = this.db.Albums.Include(x => x.Tracks).FirstOrDefault(x => x.Id == albumId);
            string albumCover = HttpUtility.UrlDecode(album.Cover);

            var tracksPrice = album.Tracks.Sum(x => x.Price);
            var tracksPriceAfterDiscount = tracksPrice - (tracksPrice * 13 / 100);

            var albumData = new StringBuilder();

            albumData.Append($"<br/><img src=\"{albumCover}\" width=\"250\" height=\"250\"><br/>");
            albumData.Append($"<p class=\"text-center\"><b>Album Name: {album.Name}</b></p>");
            albumData.Append($"<p class=\"text-center\"><b>Album Price: ${tracksPriceAfterDiscount}</b></p>");

            var tracks = album.Tracks.ToArray();

            var sbTracks = new StringBuilder();

            this.ViewBag["tracks"] = "";

            if (tracks.Length > 0)
            {
                for (int i = 1; i < tracks.Length; i++)
                {
                    var track = tracks[i];
                    sbTracks.Append($"<b>&bull; {i}.</b> <a href=\"/track/details?id={track.Id}&albumId={albumId}\">{track.Name}</a></br>");
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
                return this.View("User/Login");
            }

            this.ViewBag["albums"] = "There are currently no albums.";

            string albumsParameters = null;

            var user = db.Users.Include(x => x.Albums).FirstOrDefault(x => x.Username == this.User);

            if (user == null)
            {
                return this.View("User/Login");
            }

            var albums = user.Albums.ToArray();

            foreach (var album in albums)
            {
                albumsParameters += $"<a href=\"/album/details?id={album.Id}\">{album.Name}</a></li><br/>";
            }

            if (albumsParameters != null)
            {
                this.ViewBag["albums"] = albumsParameters;
            }

            return this.View();
        }
    }
}