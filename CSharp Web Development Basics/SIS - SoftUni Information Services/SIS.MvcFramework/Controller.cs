using SIS.HTTP.Enums;
using SIS.HTTP.Headers;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses;
using SIS.HTTP.Responses.Contracts;
using SIS.MvcFramework.Services.Contracts;
using SIS.MvcFramework.ViewEngine.Contracts;
using SIS.MvcFramework.ErrorViewModels;
using System.Diagnostics;
using System.Text;

namespace SIS.MvcFramework
{
    public abstract class Controller
    {
        protected const string VIEWS_FOLDER_PATH = "../../../Views";
        protected const string HTML_EXTENTION = ".html";
        protected const string AUTH_COOKIE_KEY = "IRunes_auth";
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
            this.errorViewModel = new ErrorViewModel();
        }

        private string GetCurrentControllerName => this.GetType().Name.Replace(DEFAULT_CONTROLER_NAME, string.Empty);

        public IHttpRequest Request { get; set; }

        public IHttpResponse Response { get; set; }

        public IUserCookieService UserCookieService { get; internal set; }

        public IViewEngine ViewEngine { get; set; }

        public ErrorViewModel errorViewModel { get; set; }

        protected string User
        {
            get
            {
                if (!this.Request.Cookies.ContainsCookie(AUTH_COOKIE_KEY))
                {
                    return null;
                }

                var cookie = this.Request.Cookies.GetCookie(AUTH_COOKIE_KEY);
                var cookieContent = cookie.Value;
                var userName = this.UserCookieService.GetUserData(cookieContent);
                return userName;
            }
        }

        protected IHttpResponse View(string viewName = null)
        {
            var allContent = this.GetViewContent(viewName, (object)null);
            this.PrepareHtmlResult(allContent);
            return this.Response;
        }

        protected IHttpResponse View<T>(string viewName = null, T model = null)
            where T : class
        {
            var allContent = this.GetViewContent(viewName, model);
            this.PrepareHtmlResult(allContent);
            return this.Response;
        }

        protected IHttpResponse View<T>(T model)
            where T : class
        {
            var allContent = this.GetViewContent(null, model);
            this.PrepareHtmlResult(allContent);
            return this.Response;
        }

        protected IHttpResponse BadRequestError<T>(string massage, string viewName, T viewModel)
            where T : class
        {
            this.errorViewModel.Massage = massage;
            var fullViewContent = GetViewContent(viewName, viewModel);

            this.PrepareHtmlResult(fullViewContent);
            this.Response.StatusCode = HttpResponseStatusCode.BadRequest;

            return this.Response;
        }

        protected IHttpResponse BadRequestError(string massage, string viewName)
        {
            this.errorViewModel.Massage = massage;
            var fullViewContent = GetViewContent(viewName, this.errorViewModel);

            this.PrepareHtmlResult(fullViewContent);
            this.Response.StatusCode = HttpResponseStatusCode.BadRequest;

            return this.Response;
        }

        private string GetViewContent<T>(string viewName, T model) 
            where T : class
        {
            var bodyFileContent = PrepareHtmlFile(viewName);
            var bodyContent = this.ViewEngine.GetHtml(BODY_VIEW_NAME, bodyFileContent, model, this.User);

            if (errorViewModel.Massage != null)
            {
                var errorFileContent = PrepareHtmlFile(ERROR_VIEW_PATH);
                var errorContent = this.ViewEngine.GetHtml(ERROR_VIEW_NAME, errorFileContent, errorViewModel, this.User);

                bodyContent = string.Concat(errorContent, bodyContent);
            }

            var layoutFileContent = PrepareHtmlFile(LAYOUT);
            var layoutContent = layoutFileContent.Replace(BODY_PLACEHOLDER, bodyContent);

            var fullContent = this.ViewEngine.GetHtml(LAYOUT, layoutContent, model, this.User);

            return fullContent;
        }

        protected string PrepareHtmlFile(string viewName)
        {
            string controlerName = GetCurrentControllerName;
            string actionName = new StackFrame(3).GetMethod().Name; //TODO: 

            string fullPath = $"{VIEWS_FOLDER_PATH}/{viewName}{HTML_EXTENTION}";

            if (viewName == null)
            {
                fullPath = $"{VIEWS_FOLDER_PATH}/{controlerName}/{actionName}{HTML_EXTENTION}";
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