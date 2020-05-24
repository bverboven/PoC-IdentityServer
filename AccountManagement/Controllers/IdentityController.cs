using Identity.Library.Defaults;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccountManagement.Controllers
{
    //[Authorize("HasPermission")]
    public class IdentityController : Controller
    {
        public IActionResult Index()
        {
            return View("Claims");
        }
        public async Task<IActionResult> UserInfo()
        {
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(IdentityDefaults.Authority);

            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            var userReq = new UserInfoRequest
            {
                Address = disco.UserInfoEndpoint,
                Token = accessToken
            };
            var userInfo = await client.GetUserInfoAsync(userReq);

            return View(userInfo.Claims);
        }

        [Authorize("IsAdmin")]
        public IActionResult IsAdmin()
        {
            return View("Policy", (object)"{ isAdmin: true }");
        }
        [Authorize("CanRead")]
        public IActionResult CanRead()
        {
            return View("Policy", (object)"{ canRead: true }");
        }
        [Authorize("Custom")]
        public IActionResult Custom()
        {
            return View("Policy", (object)"{ custom: true }");
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AccessToken()
        {
            var token = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            return View((object)token);
        }
    }
}
