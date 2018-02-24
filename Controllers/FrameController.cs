using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Frame
{
    public class FrameController : Controller
    {
        public ActionResult Event()
        {
            FrameSession session = CurrentFrameSession;
            if (session == null)
                throw new Exception("No session found");

            FrameRequest request = new Frame.FrameRequest(session, this);

            foreach (String key in Request.Form.AllKeys)
            {
                String value = Request[key];
                request.Vars.Add(key, value);
                if(key.StartsWith("frame-input-"))
                {
                    Input input = (Input)session.Find(key.Substring(12));
                    if (input != null)
                        input.ValueFromClient(value);
                }
            }

            //pause this thread?
            //respond on the current thread with a flag on how quickly to return.  or does it just return right away anyway, long polling?

            FrameRequest.Current = request;

            Act target = (Act)session.Find(Request["path"]);
            target.Handler();

            StringBuilder json = new StringBuilder();
            json.Append("{\"d\":[");
            int i = 0;

            lock (request.Response.Updates)
            {
                request.Response.ApplyChanges();
                foreach (ResponseUpdate update in request.Response.Updates)
                {
                    if (i > 0)
                        json.Append(", ");

                    update.Render(json);
                    i++;
                }
                request.Response.Updates.Clear();
            }

            json.Append("]}");

            return Content(json.ToString(), "application/json");
        }

        public ActionResult Screen()
        {
            FrameSession session = CurrentFrameSession;
            String screenId = Request["s"];
            Screen screen = (Screen)session.Find(screenId);
            return View(screen.View, screen);
        }

        public FrameSession CurrentFrameSession
        {
            get
            {
                String id = FrameSession.SessionManager.GetId(Request);
                if (id == "")
                    return null;
                else
                    return FrameSession.SessionManager.Find(id);
            }

            set
            {
                FrameSession.SessionManager.SetId(Session, Request, Response, value.Id);
            }
        }

        public string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
    }
}