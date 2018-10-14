using System;
using System.Collections.Generic;
using System.Text;

namespace CakeApp.Services.Contracts
{
    public interface IUserCookieService
    {
        string GetUserCookie(string userName);

        string GetUserData(string cookieContent);
    }
}