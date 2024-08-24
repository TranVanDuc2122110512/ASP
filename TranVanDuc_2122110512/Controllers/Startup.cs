using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;

[assembly: OwinStartup(typeof(TranVanDuc_2122110512.Startup))]

namespace TranVanDuc_2122110512
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Cấu hình Cookie Authentication
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });

            // Các cấu hình OWIN khác nếu cần
        }
    }
}
