using CommonAuthService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace DotNetFramework
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            UserManager.AddUser("User1", "Password1");
            UserManager.AddRoleToUser("User1", "Admin");

            UserManager.AddUser("User2", "Password2");
            UserManager.AddRoleToUser("User2", "Manager");

            UserManager.AddUser("User3", "Password3");
            UserManager.AddRoleToUser("User3", "User");

            foreach (var user in UserManager.GetAllUsers())
            {
                foreach (var role in UserManager.GetRolesForUser(user))
                {
                    Roles.AddUserToRole(user, role);
                }
            }
        }
    }
}
