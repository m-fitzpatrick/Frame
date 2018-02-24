using Owin;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(Frame.Startup))]
namespace Frame
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
        }
    }
}