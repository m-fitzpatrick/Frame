using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace Frame
{
    public class FrameRequest
    {
        public FrameSession Session;
        public FrameController Controller;
        public Dictionary<String, String> Vars = new Dictionary<string, string>();

        public FrameResponse Response;

        public FrameRequest(FrameSession session, FrameController controller)
        {
            Session = session;
            Controller = controller;
            Response = new FrameResponse(this);
        }

        public String RenderView(El el)
        {
            return Controller.RenderPartialViewToString(el.View, el);
        }

        public static FrameRequest Current
        {
            get
            {
                // are we sharing this data across a remote processes or something? If not, shouldn't we just use
                // a wrapper around HttpContext? Just wondering about the choice to use CallContext vs HttpContext.Current.Items
                return (FrameRequest)CallContext.GetData("FrameRequest");
            }

            set
            {
                CallContext.SetData("FrameRequest", value);
            }
        }
    }

    public delegate void FrameRequestHandler();
}