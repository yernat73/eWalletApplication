using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(eWalletApplication.Startup))]
namespace eWalletApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
            ConfigureAuth(app);
        }
    }
}
