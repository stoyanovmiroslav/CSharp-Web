﻿using IRunes.Data;
using IRunes.Services;
using IRunes.Services.Contracts;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IRunes.Controlers
{
    public abstract class BaseController
    {
        private const string VIEW_FOLDER_PATH = "../../../Views/";
        private const string FILE_EXTENTION = ".html";

        protected IRunesDbContext db;

        protected BaseController()
        {
            this.db = new IRunesDbContext();
            this.userCookieService = new UserCookieService();
            this.ViewBag = new Dictionary<string, string>();
        }

        protected IUserCookieService userCookieService { get; }

        protected Dictionary<string, string> ViewBag { get; set; }

        protected IHttpResponse View(string viewName)
        {
            string content = GetViewContent(viewName);

            this.ViewBag["body"] = content;
            
            viewName = "_LayoutLogin";

            string fullContent = GetViewContent(viewName);

            return new HtmlResult(HttpResponseStatusCode.Ok, fullContent);
        }

        protected string GetViewContent(string viewName)
        {
            var content = File.ReadAllText(VIEW_FOLDER_PATH + viewName + FILE_EXTENTION);

            foreach (var viewBagKey in ViewBag.Keys)
            {
                string placeHolder = $"{{{{{viewBagKey}}}}}";

                if (content.Contains(viewBagKey))
                {
                    content = content.Replace(placeHolder, this.ViewBag[viewBagKey]);
                }
            }

            return content;
        }

        protected IHttpResponse BadRequestError(string errorMessage)
        {
            var content = $"<h1>{errorMessage}</h1>";
            return new HtmlResult(HttpResponseStatusCode.BadRequest, content);
        }

        protected string GetUsername(IHttpRequest request)
        {
            if (!request.Cookies.ContainsCookie("IRunes_auth"))
            {
                return null;
            }
            var cookie = request.Cookies.GetCookie("IRunes_auth");
            var cookieContent = cookie.Value;
            var userName = this.userCookieService.GetUserData(cookieContent);
            return userName;
        }
    }
}