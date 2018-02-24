using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Frame
{
    public class Label : El
    {
        String mValue = "";
        public String Value
        {
            get
            {
                return mValue;
            }
            set
            {
                mValue = value;
                FrameResponse.Current.Script("$('[frame-path=\"" + Path + "\"]').text('" + mValue.Replace("'", "\'") + "')");
            }
        }

        public override string View
        {
            get
            {
                return "Label";
            }
        }
    }
}