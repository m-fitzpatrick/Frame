using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Frame
{
    public abstract class Input : El
    {
        protected String mValue = "";
        public virtual String Value
        {
            get
            {
                return mValue;
            }
            set
            {
                mValue = value;
            }
        }

        public virtual void ValueFromClient(String value)
        {
            mValue = value;
        }

        public abstract String InputType { get; }

        public override MvcHtmlString AllAttributes
        {
            get
            {
                return MvcHtmlString.Create(base.AllAttributes.ToString() + " frame-input=\"true\"");
            }
        }
    }
}