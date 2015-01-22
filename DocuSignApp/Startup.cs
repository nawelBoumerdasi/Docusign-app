using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DocuSignApp.Startup))]
namespace DocuSignApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
