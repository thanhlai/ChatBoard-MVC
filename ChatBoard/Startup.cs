using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ChatBoard.Startup))]
namespace ChatBoard
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
