using Microsoft.AspNetCore.Authorization;

namespace DotNetCore.Handlers
{
    public class CustomAuthorizeHandler : AuthorizationHandler<CustomAuthorizeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomAuthorizeRequirement requirement)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                if (!string.IsNullOrEmpty(requirement.Users))
                {
                    if (requirement.Users.Split(',').Any(user => context.User.Identity.Name.Equals(user, StringComparison.OrdinalIgnoreCase)))
                    {
                        context.Succeed(requirement);
                    }
                }

                if (!string.IsNullOrEmpty(requirement.Roles))
                {
                    if (requirement.Roles.Split(',').Any(role => context.User.IsInRole(role)))
                    {
                        context.Succeed(requirement);
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}
