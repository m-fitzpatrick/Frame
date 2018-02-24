using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Frame;

namespace FrameExample
{
    public class Login : Screen
    {
        public Text UserName;
        public Text Password;
        public Button LoginButton;
        public Login()
        {
            Title.Value = "Pickle Rick!";
            UserName = (Text)Body.Add(new Text());
            Password = (PasswordText)Body.Add(new PasswordText());
            LoginButton = (Button)Body.Add(new Button("Log In", new FrameRequestHandler(LoginClicked)));
        }

        void LoginClicked()
        {
            ExampleScreen example = new ExampleScreen();
            FrameSession.Current.Add(example);
            FrameResponse.Current.Redirect(example.Url);
        }
    }
}