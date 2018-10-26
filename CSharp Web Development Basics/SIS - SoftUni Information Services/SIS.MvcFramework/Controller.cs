using SIS.HTTP.Enums;
using SIS.HTTP.Headers;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses;
using SIS.HTTP.Responses.Contracts;
using SIS.MvcFramework.ViewEngine;
using SIS.MvcFramework.Services.Contracts;
using SIS.MvcFramework.ViewEngine.Contracts;
using System.Text;
using System.Runtime.CompilerServices;
using System;

namespace SIS.MvcFramework
{
    public abstract class Controller
    {
        protected const string SESSION_KEY = "username";
        protected const string VIEWS_FOLDER_PATH = "../../../Views";
        protected const string HTML_EXTENTION = ".html";
        protected const string AUTH_COOKIE_KEY = "_auth";
        private const string DEFAULT_CONTROLER_NAME = "Controller";
        private const string LAYOUT = "_Layout";
        private const string ERROR_VIEW_PATH = "Error/Error";
        private const string ERROR_VIEW_NAME = "Error";
        private const string BODY_PLACEHOLDER = "@RenderBody()";
        private const string BODY_VIEW_NAME = "Body";

        public Controller()
        {
            this.Response = new HttpResponse { StatusCode = HttpResponseStatusCode.Ok };
            this.ViewEngine = new SIS.MvcFramework.ViewEngine.ViewEngine();
            this.ErrorViewModel = new ErrorViewModel();
        }

        private string GetCallingControllerName => this.GetType().Name.Replace(DEFAULT_CONTROLER_NAME, string.Empty);
        private Type GetCallingControllerType => this.GetType();

        public IHttpRequest Request { get; set; }

        public IHttpResponse Response { get; set; }

        public IUserCookieService UserCookieService { get; internal set; }

        public IViewEngine ViewEngine { get; set; }

        public ErrorViewModel ErrorViewModel { get; set; }

        protected UserModel User
        {
            get
            {
                if (!this.Request.Session.ContainsParameter(SESSION_KEY)
                    || !this.Request.Cookies.ContainsCookie(AUTH_COOKIE_KEY))
                {
                    return new UserModel();
                }

                var cookie = this.Request.Cookies.GetCookie(AUTH_COOKIE_KEY);
                var cookieContent = cookie.Value;
                var userName = this.UserCookieService.GetUserData(cookieContent);

                UserModel userModel = (UserModel)Request.Session.GetParameter(SESSION_KEY);

                return userModel;
            }
        }

        protected IHttpResponse View([CallerMemberName] string viewName = "")
        {
            var allContent = this.GetViewContent(viewName, (object)null);
            this.PrepareHtmlResult(allContent);
            return this.Response;
        }

        protected IHttpResponse View<T>(T model = null, [CallerMemberName] string viewName = "")
            where T : class
        {
            var allContent = this.GetViewContent(viewName, model);
            this.PrepareHtmlResult(allContent);
            return this.Response;
        }

        protected IHttpResponse BadRequestError<T>(string massage, string viewName, T viewModel)
            where T : class
        {
            this.ErrorViewModel.Massage = massage;
            var fullViewContent = GetViewContent(viewName, viewModel);

            this.PrepareHtmlResult(fullViewContent);
            this.Response.StatusCode = HttpResponseStatusCode.BadRequest;

            return this.Response;
        }

        protected IHttpResponse BadRequestError(string massage, string viewName = "/error/basicerror")
        {
            this.ErrorViewModel.Massage = massage;
            var fullViewContent = GetViewContent(viewName, this.ErrorViewModel);

            this.PrepareHtmlResult(fullViewContent);
            this.Response.StatusCode = HttpResponseStatusCode.BadRequest;

            return this.Response;
        }

        private string GetViewContent<T>(string viewName, T model)
            where T : class
        {
            var bodyFileContent = PrepareHtmlFile(viewName);
            var bodyContent = this.ViewEngine.GetHtml(BODY_VIEW_NAME, bodyFileContent, model, this.User);

            if (ErrorViewModel.Massage != null)
            {
                var errorFileContent = PrepareHtmlFile(ERROR_VIEW_PATH);
                var errorContent = this.ViewEngine.GetHtml(ERROR_VIEW_NAME, errorFileContent, ErrorViewModel, this.User);

                bodyContent = string.Concat(errorContent, bodyContent);
            }

            var layoutFileContent = PrepareHtmlFile(LAYOUT);
            var layoutContent = layoutFileContent.Replace(BODY_PLACEHOLDER, bodyContent);

            var fullContent = this.ViewEngine.GetHtml(LAYOUT, layoutContent, model, this.User);

            return fullContent;
        }

        protected string PrepareHtmlFile(string viewName)
        {
            if (viewName.StartsWith("/"))
            {
                viewName = viewName.Substring(1);
            }

            string fullPath = $"{VIEWS_FOLDER_PATH}/{viewName}{HTML_EXTENTION}";

            if (!viewName.Contains("/") && !viewName.Contains(LAYOUT))
            {
                fullPath = $"{VIEWS_FOLDER_PATH}/{GetCallingControllerName}/{viewName}{HTML_EXTENTION}";
            }

            return System.IO.File.ReadAllText(fullPath);
        }

        private void PrepareHtmlResult(string fullViewContent)
        {
            this.Response.Headers.Add(new HttpHeader(HttpHeader.CONTENT_TYPE, "text/html"));
            this.Response.Content = Encoding.UTF8.GetBytes(fullViewContent);
        }

        protected IHttpResponse Redirect(string location)
        {
            this.Response.Headers.Add(new HttpHeader(HttpHeader.LOCATION, location));
            this.Response.StatusCode = HttpResponseStatusCode.SeeOther;
            return this.Response;
        }

        protected IHttpResponse File(byte[] content)
        {
            this.Response.Headers.Add(new HttpHeader(HttpHeader.CONTENT_LENGTH, content.Length.ToString()));
            this.Response.Headers.Add(new HttpHeader(HttpHeader.CONTENT_DISPOSITION, "inline"));
            this.Response.Content = content;
            return this.Response;
        }

        protected IHttpResponse Text(string content)
        {
            this.Response.Headers.Add(new HttpHeader(HttpHeader.CONTENT_TYPE, "text/plain; charset=utf-8"));
            this.Response.Content = Encoding.UTF8.GetBytes(content);
            return this.Response;
        }
    }
}