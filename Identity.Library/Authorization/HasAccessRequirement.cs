using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Identity.Library.Authorization
{
    public class HasAccessRequirement : IAuthorizationRequirement
    {
    }

    public class HasAccessRequirementHandler : AuthorizationHandler<HasAccessRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasAccessRequirement requirement)
        {
            var claims = context.User.Claims;

            context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
