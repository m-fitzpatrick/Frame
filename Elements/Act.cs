using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Frame
{
    public class Act : El
    {
        public FrameRequestHandler Handler;

        public Act(FrameRequestHandler handler)
        {
            Handler = handler;
        }

        public MvcHtmlString Script
        {
            get
            {
                return MvcHtmlString.Create("FrameEvent('" + Path + "');");
            }
        }
    }
}