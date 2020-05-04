using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Sts.Quickstart.Account
{
    [AllowAnonymous]
    public class LockoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
