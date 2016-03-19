using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AccessControlSystem.Startup))]
namespace AccessControlSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
