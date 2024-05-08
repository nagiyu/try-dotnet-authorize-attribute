using Microsoft.AspNetCore.Authorization;

namespace DotNetCore.Handlers
{
    public class CustomAuthorizeRequirement : IAuthorizationRequirement
    {
        public string Users { get; private set; }
        public string Roles { get; private set; }

        public CustomAuthorizeRequirement(string users, string roles)
        {
            Users = users;
            Roles = roles;
        }
    }
}
