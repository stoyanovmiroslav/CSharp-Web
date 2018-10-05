using SIS.HTTP.Cookies.Contracts;
using SIS.HTTP.Exceptions;
using System;
using System.Collections;
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

        public bool ContainsCookie(string key)
        {
            return this.cookies.ContainsKey(key);
        }

        public HttpCookie GetCookie(string key)
        {
            if (!ContainsCookie(key))
            {
                throw new BadRequestException();
            }

            return this.cookies[key];
        }

        public IEnumerator<HttpCookie> GetEnumerator()
        {
            foreach (var cookie in this.cookies)
            {
                yield return cookie.Value;
            }
        }

        public bool HasCookies()
        {
            return this.cookies.Count > 0;
        }

        public override string ToString()
        {
            return string.Join("; ", cookies.Values);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}