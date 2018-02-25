using System;
using System.Collections.Concurrent;
using System.Web;

namespace Frame
{
    public class FrameSession : IDisposable
    {
        public static FrameSessionManager SessionManager = new MemoryCookieFrameSessionManager();

        public String Id = Guid.NewGuid().ToString();

        public ConcurrentDictionary<string, Screen> Screens = new ConcurrentDictionary<string, Screen>();
        public Screen Add(Screen s)
        {
            Screens.TryAdd(s.Id, s);

            return s;
        }

        public El Find(String path)
        {
            El ret = null;

            if (!string.IsNullOrEmpty(path))
            {
                String[] ids = path.Split('.');
                Screen screen = Screens[ids[0]];

                if (ids.Length == 1)
                    return screen;

                ret = screen.Find(ids[1]);

                for (int i = 2; i < ids.Length; i++)
                {
                    ret = ret.Find(ids[i]);
                }
            }

            return ret;
        }

        public void Dispose()
        {
            //dispose screens?
            // if they implement IDisposable
            Screens.Clear();
            Screens = null;
        }

        public static FrameSession Current
        {
            get
            {
                return FrameRequest.Current.Session;
            }
        }
    }

    public abstract class FrameSessionManager
    {
        public abstract FrameSession Add(FrameSession session);
        public abstract FrameSession Find(String sessionId);
        public abstract void Remove(String sessionId);

        public abstract String GetId(HttpRequestBase request);

        public abstract void SetId(HttpSessionStateBase session, HttpRequestBase request, HttpResponseBase response, String sessionId);
    }

    public class MemoryCookieFrameSessionManager : FrameSessionManager
    {
        public ConcurrentDictionary<String, FrameSession> Sessions = new ConcurrentDictionary<string, FrameSession>();

        public override FrameSession Add(FrameSession session)
        {
            //lock (Sessions)
            // If thread safety is the issue here, ConcurrentDictionary will work better
            Sessions.TryAdd(session.Id, session);

            return session;
        }

        public override FrameSession Find(string sessionId)
        {
            if (Sessions.ContainsKey(sessionId))
            {
                return Sessions[sessionId];
            }

            return null;
        }

        public override void Remove(string sessionId)
        {
            FrameSession outSession;
            if (Sessions.TryRemove(sessionId, out outSession))
            {
                outSession.Dispose();
            }
        }

        public override void SetId(HttpSessionStateBase session, HttpRequestBase request, HttpResponseBase response, String sessionId)
        {
            HttpCookie sessionCookie = new HttpCookie("FrameSessionId");
            sessionCookie.Expires = DateTime.Now.AddYears(1000);
            response.Cookies.Add(sessionCookie);
        }

        public override string GetId(HttpRequestBase request)
        {
            HttpCookie sessionCookie = request.Cookies["FrameSessionId"];
            if (sessionCookie == null)
            {
                return string.Empty;
            }
            
            return sessionCookie.Value;
        }
    }
}