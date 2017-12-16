using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ADV.InternetCrawler.ControlPanel.Startup))]
namespace ADV.InternetCrawler.ControlPanel
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
