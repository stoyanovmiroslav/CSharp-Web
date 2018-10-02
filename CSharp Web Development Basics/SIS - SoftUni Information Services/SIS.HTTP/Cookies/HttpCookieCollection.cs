using SIS.HTTP.Cookies.Contracts;
using SIS.HTTP.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Cookies
{
    public class HttpCookieCollection : IHttpCookieCollection
    {
        private IDictionary<string, HttpCookie> cookies;

        public HttpCookieCollection()
        {
            this.cookies = new Dictionary<string, HttpCookie>();
        }

        public void Add(HttpCookie httpCookie)
        {
            this.cookies[httpCookie.Key] = httpCookie;
        }

        public bool ConstainsCookie(string key)
        {
            return this.cookies.ContainsKey(key);
        }

        public HttpCookie GetCookie(string key)
        {
            if (!ConstainsCookie(key))
            {
                throw new BadRequestException();
            }

            return this.cookies[key];
        }

        public bool HasCookies()
        {
            return this.cookies.Count > 0;
        }

        public override string ToString()
        {
            return string.Join("; ", cookies.Values);
        }
    }
}