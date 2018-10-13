using SIS.HTTP.Session.Contracts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Session
{
    public class HttpSessionStorage
    {
        public const string SessionKey = "SIS_ID";

        private static readonly ConcurrentDictionary<string, HttpSession> sessions = new ConcurrentDictionary<string, HttpSession>();

        public static IHttpSession GetSession(string id)
        {
            return sessions.GetOrAdd(id, _ => new HttpSession(id));
        }
    }
}
