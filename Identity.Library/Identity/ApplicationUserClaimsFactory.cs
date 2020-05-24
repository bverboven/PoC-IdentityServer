using System.Security.Claims;
using System.Threading.Tasks;
using Identity.Library.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Identity.Library.Identity
{
    public class ApplicationUserClaimsFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        public ApplicationUserClaimsFactory(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> options)
            : base(userManager, roleManager, options)
        {
        }


        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            if (!string.IsNullOrWhiteSpace(user.GivenName))
            {
                identity.AddClaim(new Claim("given_name", user.GivenName));
            }
            if (!string.IsNullOrWhiteSpace(user.FamilyName))
            {
                identity.AddClaim(new Claim("family_name", user.FamilyName));
            }

            return identity;
        }
    }
}