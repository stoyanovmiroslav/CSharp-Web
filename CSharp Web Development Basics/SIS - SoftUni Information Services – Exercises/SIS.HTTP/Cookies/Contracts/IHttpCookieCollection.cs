using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Cookies.Contracts
{
    public interface IHttpCookieCollection : IEnumerable<HttpCookie>
    {
        void Add(HttpCookie httpCookie);

        bool ContainsCookie(string key);

        HttpCookie GetCookie(string key);

        bool HasCookies();
    }
}
