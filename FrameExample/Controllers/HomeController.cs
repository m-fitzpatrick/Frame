using Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrameExample
{
    public class HomeController : FrameController
    {
        public ActionResult Index()
        {
            FrameSession existingSession = CurrentFrameSession;
            if (existingSession != null)
            {
                FrameRequest.Current = new FrameRequest(existingSession, this);
                return Redirect(((ExampleScreen)existingSession.Add(new ExampleScreen())).Url);
            }

            FrameSession session = FrameSession.SessionManager.Add(new FrameSession());
            CurrentFrameSession = session;

            Response.Cookies.Add(new HttpCookie("FrameSessionId", session.Id));
            FrameRequest.Current = new FrameRequest(session, this);
            return View("Login", session.Add(new Login()));
        }
    }
}