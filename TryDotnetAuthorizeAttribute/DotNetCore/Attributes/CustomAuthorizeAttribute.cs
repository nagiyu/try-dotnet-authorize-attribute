using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace DotNetFramework.Attributes
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        // ユーザー名またはロール名を指定します。
        public CustomAuthorizeAttribute()
        {
            Users = "User1,User2";
            Roles = "Admin,Manager";
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            // 遷移元画面の URL をセッションに保存
            filterContext.HttpContext.Session.Add("LogOnReturnUrl", filterContext.HttpContext.Request.RawUrl);
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // 基底クラスのAuthorizeCoreメソッドを呼び出す
            bool isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }

            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            // ユーザーが認証されているかどうかを確認します。
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }

            // ユーザー名またはロール名が指定されている場合、ユーザーがそれらに一致するかどうかを確認します。
            if (Users.Length > 0 && !Users.Split(',').Any(user => httpContext.User.Identity.Name.Equals(user, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

            if (Roles.Length > 0 && !Roles.Split(',').Any(role => httpContext.User.IsInRole(role)))
            {
                return false;
            }

            return true;
        }
    }
}