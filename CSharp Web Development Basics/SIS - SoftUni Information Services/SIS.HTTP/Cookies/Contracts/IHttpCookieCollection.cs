using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Cookies.Contracts
{
    public interface IHttpCookieCollection
    {
        void Add(HttpCookie httpCookie);

        bool ConstainsCookie(string key);

        HttpCookie GetCookie(string key);

        bool HasCookies();
    }
}
