using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WordScrambler.Startup))]
namespace WordScrambler
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
