using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace DotNetFramework.Attributes
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        // ユーザー名またはロール名を指定します。
        public CustomAuthorizeAttribute()
        {
            Users = "User1,User2";
            Roles = "Admin,Manager";
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // 遷移元画面の URL をセッションに保存
            context.HttpContext.Session.SetString("LogOnReturnUrl", context.HttpContext.Request.Path);
        }

        public bool AuthorizeCore(HttpContext httpContext)
        {
            // 基底クラスのAuthorizeCoreメソッドを呼び出す
            bool isAuthorized = httpContext.User.Identity.IsAuthenticated;
            if (!isAuthorized)
            {
                return false;
            }

            ArgumentNullException.ThrowIfNull(httpContext);

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