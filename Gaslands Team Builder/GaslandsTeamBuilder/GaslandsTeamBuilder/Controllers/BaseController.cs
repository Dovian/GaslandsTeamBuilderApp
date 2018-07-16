using GaslandsTeamBuilderCore;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GaslandsTeamBuilder.Controllers
{
    public class BaseController : Controller
    {
        internal AppUserState _AppUserState;
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            // Grab the user's login information from Identity
            AppUserState appUserState = new AppUserState();
            if (User is ClaimsPrincipal)
            {
                var user = User as ClaimsPrincipal;
                var claims = user.Claims.ToList();

                var userStateString = GetClaim(claims, "userState");

                if (!string.IsNullOrEmpty(userStateString))
                    appUserState = appUserState.FromString(userStateString);
            }
            _AppUserState = appUserState;

            ViewData["UserState"] = _AppUserState;
        }
        
        public void IdentitySignin(AppUserState appUserState, bool isPersistent = false)
        {
            var claims = new List<Claim>();

            // create required claims
            claims.Add(new Claim(ClaimTypes.NameIdentifier, appUserState.UserId.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, appUserState.Username));
            
            claims.Add(new Claim("userState", appUserState.ToString()));

            var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            AuthenticationManager.SignIn(new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = isPersistent,
                ExpiresUtc = DateTime.UtcNow.AddDays(7)
            }, identity);
        }

        public void IdentitySignout()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie,
                                            DefaultAuthenticationTypes.ExternalCookie);
        }

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        private string GetClaim(List<Claim> claims, string type)
        {
            return claims.SingleOrDefault(c => c.Type == type) == null ? "" : claims.SingleOrDefault(c => c.Type == type).Value;
        }
    }
}