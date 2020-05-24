using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AccountApi.Controllers
{
    [ApiController]
    [Route("api/identity")]
    public class IdentityController : ControllerBase
    {
        [HttpGet("claims")]
        public IActionResult Get()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value });
            return Ok(claims);
        }
    }
}
