using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Frame
{
    public class FrameResponse
    {
        public ConcurrentBag<ResponseUpdate> Updates = new ConcurrentBag<ResponseUpdate>();
        public FrameRequest Request;

        public FrameResponse(FrameRequest request)
        {
            Request = request;
        }

        public void Redirect(String url)
        {
            Updates.Add(new UpdateScript("window.location = '" + url + "'"));
        }

        public void Script(String script)
        {
            Updates.Add(new UpdateScript(script));
        }

        public bool HasChanged(String id)
        {
            return AddedEls.ContainsKey(id) || UpdatedEls.ContainsKey(id) || RemovedEls.ContainsKey(id);
        }

        public Dictionary<String, El> AddedEls = new Dictionary<string, El>();
        public Dictionary<String, El> UpdatedEls = new Dictionary<string, El>();
        public Dictionary<String, ElTag> RemovedEls = new Dictionary<string, ElTag>();

        public void Added(El el)
        {
            if (el is Body || el is Act)
                return;

            if(el.Parent != null && HasChanged(el.Parent.Id))
                return;

            AddedEls.Add(el.Id, el);
        }

        public void Updated(El el)
        {
            if (el.Parent != null && HasChanged(el.Parent.Id))
                return;

            UpdatedEls.Add(el.Id, el);
        }

        public void Removed(El el)
        {
            if (el.Parent != null && HasChanged(el.Parent.Id))
                return;

            RemovedEls.Add(el.Id, new ElTag(el));
        }

        public void ApplyChanges()
        {
            foreach(KeyValuePair<String, El> k in AddedEls)
            {
                if (k.Value.Parent != null && HasChanged(k.Value.Parent.Id))
                    continue;
                Updates.Add(new UpdateAdd(k.Value.Parent.Path, FrameRequest.Current.RenderView(k.Value)));
            }

            foreach (KeyValuePair<String, El> k in UpdatedEls)
            {
                if (k.Value.Parent != null && HasChanged(k.Value.Parent.Id))
                    continue;
                Updates.Add(new UpdateReplace(k.Value.Path, FrameRequest.Current.RenderView(k.Value)));
            }

            foreach (KeyValuePair<String, ElTag> k in RemovedEls)
            {
                if (!String.IsNullOrEmpty(k.Value.ParentId) && HasChanged(k.Value.ParentId))
                    continue;

                Updates.Add(new UpdateRemove(k.Value.Path));
            }
        }

        public static FrameResponse Current
        {
            get
            {
                return FrameRequest.Current.Response;
            }
        }
    }

    public class ElTag
    {
        public String Id;
        public String ParentId;
        public String Path;

        public ElTag(El el)
        {
            Id = el.Id;
            ParentId = el.Parent == null ? "" : el.Parent.Id;
            Path = el.Path;
        }
    }

    public abstract class ResponseUpdate
    {
        public abstract object Render();
    }

    public class UpdateScript : ResponseUpdate
    {
        public String Script;
        public bool ValidOnInit;

        public UpdateScript(String script, bool validOnInit = false)
        {
            Script = script;
            ValidOnInit = validOnInit;
        }

        public override object Render()
        {
            return new
                    {
                        name = "script",
                        script = Script
                    };
        }
    }

    public class UpdateReplace : ResponseUpdate
    {
        public String Path;
        public String Html;

        public UpdateReplace(String path, String html)
        {
            Path = path;
            Html = html;
        }

        public override object Render()
        {
            return new
                    {
                        name = "replace",
                        path = Path,
                        html = Html
                    };
        }
    }

    public class UpdateRemove : ResponseUpdate
    {
        public String Path;

        public UpdateRemove(String path)
        {
            Path = path;
        }

        public override object Render()
        {
            return new
                    {
                        name = "remove",
                        path = Path
                    };
        }
    }

    public class UpdateAdd : ResponseUpdate
    {
        public String Path;
        public String Html;

        public UpdateAdd(String path, String html)
        {
            Path = path;
            Html = html;
        }

        public override object Render()
        {
            return new
                    {
                        name = "add",
                        path = Path,
                        html = Html
                    };
        }
    }
}
