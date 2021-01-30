using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GlobalAsset.Startup))]
namespace GlobalAsset
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
