using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Qalsql.Startup))]
namespace Qalsql
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
