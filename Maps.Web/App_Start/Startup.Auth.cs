using Maps.Data;
using Maps.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using Owin.Security.Providers.GitHub;
using System;
using System.Configuration;

namespace Maps
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(MapsDbContext.Create);
            app.CreatePerOwinContext<UserManager>(UserManager.Create);
            app.CreatePerOwinContext<SignInManager>(SignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Index"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<UserManager, User>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            app.UseGitHubAuthentication(
                clientId: ConfigurationManager.AppSettings["GitHubClientId"],
                clientSecret: ConfigurationManager.AppSettings["GitHubClientSecret"]);

            app.UseMicrosoftAccountAuthentication(
                clientId: ConfigurationManager.AppSettings["MicrosoftClientId"],
                clientSecret: ConfigurationManager.AppSettings["MicrosoftClientSecret"]);

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            app.UseFacebookAuthentication(
               appId: ConfigurationManager.AppSettings["FacebookAppId"],
               appSecret: ConfigurationManager.AppSettings["FacebookAppSecret"]);

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = ConfigurationManager.AppSettings["GoogleClientId"],
                ClientSecret = ConfigurationManager.AppSettings["GoogleClientSecret"]
            });
        }
    }
}