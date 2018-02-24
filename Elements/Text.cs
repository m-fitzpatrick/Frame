using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Frame
{
    public class Text : Input
    {
        public String ClientOnEnter = "";

        public String RenderClientOnEnter()
        {
            if (ClientOnEnter == "")
                return "";
            return " onkeydown=\"if(event.keyCode == 13) " + ClientOnEnter + "\"";
        }

        public override string View
        {
            get
            {
                return "Text";
            }
        }

        public override string InputType
        {
            get
            {
                return "text";
            }
        }

        public override string Value
        {
            get
            {
                return base.Value;
            }

            set
            {
                base.Value = value;
                FrameResponse.Current.Script("$('[frame-path=\"" + Path + "\"]').val('" + mValue.Replace("'", "\'") + "')");
            }
        }

        public String FocusScript
        {
            get
            {
                return SelectThis + ".focus()";
            }
        }

        public Text Focus()
        {
            FrameResponse.Current.Script(FocusScript);
            return this;
        }
    }

    public class PasswordText : Text
    {
        public override string InputType
        {
            get
            {
                return "password";
            }
        }
    }
}