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