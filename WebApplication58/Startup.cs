using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebApplication58.Startup))]
namespace WebApplication58
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
