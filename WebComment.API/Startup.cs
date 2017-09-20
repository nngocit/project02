using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebComment.API.Startup))]
namespace WebComment.API
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
