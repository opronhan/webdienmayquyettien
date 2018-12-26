using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DienMayQT.Startup))]
namespace DienMayQT
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
