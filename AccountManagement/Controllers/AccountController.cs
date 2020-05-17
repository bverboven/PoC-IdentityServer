using System;
using Identity.Library.Defaults;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountManagement.Controllers
{
    public class AccountController : ControllerBase
    {
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = "/")
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                // a returnUrl must be specified to prevent infinite redirection loop
                throw new ArgumentNullException(nameof(returnUrl));
            }

            return Challenge(new AuthenticationProperties { RedirectUri = returnUrl }, IdentityDefaults.AuthenticationScheme);
        }

        public IActionResult Logout()
        {
            return SignOut(CookieAuthenticationDefaults.AuthenticationScheme, IdentityDefaults.AuthenticationScheme);
        }
    }
}
