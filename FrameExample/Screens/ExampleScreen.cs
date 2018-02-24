using Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrameExample
{
    public class ExampleScreen : Screen
    {
        public ListOfStrings StringList;
        public Text NewStringBox;
        public Button AddStringButton;

        public ExampleScreen()
        {
            Title.Value = "Example";
            StringList = (ListOfStrings)Add(new ListOfStrings(this));
            AddStringButton = (Button)Add(new Button("Add", new FrameRequestHandler(StringAddedHandler)));
            NewStringBox = (Text)Add(new Text());
            NewStringBox.ClientOnEnter = "ClickFrameElement('" + AddStringButton.Path + "')";
        }

        public void StringAddedHandler()
        {
            StringList.AddString(NewStringBox.Value);
        }

        public override string View
        {
            get
            {
                return "/Views/Home/Example.cshtml";
            }
        }
    }

    public class ListOfStrings : El
    {
        public Label CountLabel;
        public ListOfStringsContent Content;
        ExampleScreen parentScreen;

        public ListOfStrings(ExampleScreen pScreen)
        {
            CountLabel = (Label)Add(new Label { Value = "0" });
            Content = (ListOfStringsContent)Add(new ListOfStringsContent());
            Content.OnAdded += new ElChangedHandler(Content_OnChange);
            Content.OnRemoved += new ElChangedHandler(Content_OnChange);
            parentScreen = pScreen;
        }

        private void Content_OnChange(El el)
        {
            CountLabel.Value = Content.Els.Count.ToString();
            parentScreen.NewStringBox.Focus().Value = "";
        }

        public StringItem AddString(String value)
        {
            return (StringItem)Content.Add(new StringItem(value));
        }

        public override string View
        {
            get
            {
                return "/Views/Home/ListOfStrings.cshtml";
            }
        }
    }

    public class ListOfStringsContent : El
    {
        public override string View
        {
            get
            {
                return "/Views/Home/ListOfStringsContent.cshtml";
            }
        }
    }

    public class StringItem : El
    {
        public String StringValue;
        public Act RemoveString;

        public StringItem(String value)
        {
            StringValue = value;
            RemoveString = (Act)Add(new Act(new FrameRequestHandler(RemoveStringHandler)));
        }

        public override string View
        {
            get
            {
                return "/Views/Home/StringItem.cshtml";
            }
        }

        public void RemoveStringHandler()
        {
            Parent.Remove(this);
        }
    }
}