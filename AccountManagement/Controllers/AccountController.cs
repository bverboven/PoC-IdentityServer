using Identity.Library.Defaults;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AccountManagement.Controllers
{
    public class AccountController : Controller
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

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
