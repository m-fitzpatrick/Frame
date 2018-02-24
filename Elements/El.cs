using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Frame
{
    public class El
    {
        public String Id = Guid.NewGuid().ToString();
        public Dictionary<String, El> Els = new Dictionary<String, El>();
        public El Parent;
        
        public String Path
        {
            get
            {
                if (Parent == null)
                    return Id;
                else
                    return Parent.Path + "." + Id; 
            }
        }

        public String PathAttribute
        {
            get
            {
                return "frame-path=\"" + Path + "\"";
            }
        }

        public virtual MvcHtmlString AllAttributes
        {
            get
            {
                return MvcHtmlString.Create(PathAttribute);
            }
        }

        public List<El> ElsList
        {
            get
            {
                List<El> ret = new List<El>();
                foreach (KeyValuePair<String, El> k in Els)
                    ret.Add(k.Value);
                return ret;
            }
        }

        public El Add(El el)
        {
            Els.Add(el.Id, el);
            el.Parent = this;
            FrameResponse.Current.Added(el);
            Added(el);
            return el;
        }

        public El Remove(El el)
        {
            FrameResponse.Current.Removed(el);  //this has to be first to maintain the path            
            Els.Remove(el.Id);
            el.Parent = null;
            Removed(el);
            return el;
        }

        public event ElChangedHandler OnAdded;
        public virtual void Added(El el)
        {
            OnAdded?.Invoke(el);
        }

        public event ElChangedHandler OnRemoved;
        public virtual void Removed(El el)
        {
            OnRemoved?.Invoke(el);
        }

        public virtual String View
        {
            get
            {
                return "El";
            }
        }

        public El Find(String id)
        {
            return Els[id];
        }

        public String SelectThis
        {
            get
            {
                return "$(\"[frame-path='" + Path + "']\")";
            }
        }
    }

    public delegate void ElChangedHandler(El el);
}