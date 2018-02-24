using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Frame
{
    public class Screen : El
    {
        public FrameSession Session;
        public Text Title;
        public Body Body;

        public Screen()
        {
            Title = (Text)Add(new Text());
            Body = (Body)Add(new Body());
        }

        public List<String> ScriptFiles = new List<string>();
        public String RenderScriptFiles(String indent = "")
        {
            return RenderIndentedList(ScriptFiles, indent);
        }

        public List<String> StylesheetFiles = new List<string>();
        public String RenderStylesheetFiles(String indent = "")
        {
            return RenderIndentedList(StylesheetFiles, indent);
        }

        public virtual String Url
        {
            get
            {
                return "Frame/Screen/?s=" + Id;
            }
        }

        static String RenderIndentedList(List<String> list, String indent)
        {
            StringBuilder sb = new StringBuilder();
            foreach (String s in list)
                sb.AppendLine(indent + s);
            return sb.ToString();
        }
    }
}