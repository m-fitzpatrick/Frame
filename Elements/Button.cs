using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Frame
{ 
    public class Button : El
    {

        public String Value = "";
        public Act Click;

        public Button(String value, FrameRequestHandler clickHandler)
        {
            Value = value;
            Click = (Act)Add(new Act(clickHandler));
        }

        public override string View
        {
            get
            {
                return "Button";
            }
        }
    }
}