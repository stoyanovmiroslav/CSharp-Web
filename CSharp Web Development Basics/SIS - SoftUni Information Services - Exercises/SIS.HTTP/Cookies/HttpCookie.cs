using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SIS.HTTP.Cookies
{
    public class HttpCookie
    {
        private const int HttpCookieDefaultExpirationDays = 3;
        private const string HttpCookieDefaultPath = "/";

        public HttpCookie(string key, string value, int expires = HttpCookieDefaultExpirationDays, string path = HttpCookieDefaultPath)
        {
            this.Key = key;
            this.Value = value;
            this.IsNew = true;
            this.Path = path;
            this.Expires = DateTime.UtcNow.AddDays(expires);
        }

        public HttpCookie(string key, string value, bool isNew, int expires = HttpCookieDefaultExpirationDays, string path = HttpCookieDefaultPath)
            : this(key, value, expires)
        {
            this.IsNew = isNew;
            
        }

        public string Path { get; set; }

        public bool HttpOnly { get; set; } = true;

        public string Key { get; set; }

        public string Value { get; set; }

        public DateTime Expires { get; set; }

        public bool IsNew { get; set; }

        public void Delete()
        {
            this.Expires = DateTime.UtcNow.AddDays(-1);
        } 

        public override string ToString()
        {
            string expires = this.Expires.ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture);

            StringBuilder sb = new StringBuilder();

            sb.Append($"{this.Key}={this.Value}; Expires={expires}");
           
            if (this.HttpOnly)
            {
                sb.Append("; HttpOnly");
            }

            sb.Append($"; Path={this.Path}");

            return sb.ToString();
        }
    }
}