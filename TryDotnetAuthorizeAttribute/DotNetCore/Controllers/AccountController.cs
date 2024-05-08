using CommonAuthService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Microsoft.AspNetCore.Mvc;

namespace DotNetFramework.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return Content("ユーザー名またはパスワードが入力されていません");
            }

            if (UserManager.UserExists(username))
            {
                // パスワードが正しいかどうかをチェック
                if (UserManager.IsPasswordCorrect(username, password))
                {
                    // TODO ASP.NET membership should be replaced with ASP.NET Core identity. For more details see https://docs.microsoft.com/aspnet/core/migration/proper-to-2x/membership-to-core-identity.
                    FormsAuthentication.SetAuthCookie(username, true);
                }
                else
                {
                    return Content("パスワードが正しくありません");
                }
            }
            else
            {
                return Content("ユーザーが存在しません");
            }

            return Content("ログインしました");
        }

        public ActionResult Logout()
        {
            // TODO ASP.NET membership should be replaced with ASP.NET Core identity. For more details see https://docs.microsoft.com/aspnet/core/migration/proper-to-2x/membership-to-core-identity.
            FormsAuthentication.SignOut();
            return Content("ログアウトしました");
        }
    }
}