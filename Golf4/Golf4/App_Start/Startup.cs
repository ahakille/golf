using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Golf4.App_Start;

[assembly: OwinStartup(typeof(Startup))]
namespace Golf4.App_Start
{
    public partial class Startup
    {
        /// <summary>
        /// Skapar en login cookie!
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {


            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ApplicationCookie",
                LoginPath = new PathString("/account/index")
            });
        }
    }
}