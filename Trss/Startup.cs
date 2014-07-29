using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Trss.Startup))]
namespace Trss
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
