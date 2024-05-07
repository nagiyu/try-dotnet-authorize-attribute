using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using CommonAuthService;

namespace DotNetFramework.Providers
{
    public class CustomRoleProvider : RoleProvider
    {
        public override string ApplicationName { get; set; }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            foreach (var username in usernames)
            {
                foreach (var roleName in roleNames)
                {
                    UserManager.AddRoleToUser(username, roleName);
                }
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            return UserManager.GetRolesForUser(username).ToArray();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            return UserManager.GetAllUsers().Where(user => UserManager.IsUserInRole(user, roleName)).ToArray();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            return UserManager.IsUserInRole(username, roleName);
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            foreach (var username in usernames)
            {
                foreach (var roleName in roleNames)
                {
                    var rolesForUser = UserManager.GetRolesForUser(username);
                    if (rolesForUser.Contains(roleName))
                    {
                        rolesForUser.Remove(roleName);
                    }
                }
            }
        }

        public override bool RoleExists(string roleName)
        {
            var allRoles = UserManager.GetAllUsers().SelectMany(UserManager.GetRolesForUser).Distinct();
            return allRoles.Contains(roleName);
        }
    }
}