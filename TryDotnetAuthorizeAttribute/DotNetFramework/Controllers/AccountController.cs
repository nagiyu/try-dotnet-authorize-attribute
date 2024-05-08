using CommonAuthService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

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
                Console.WriteLine("ユーザー名またはパスワードが入力されていません");
                return RedirectToAction("Index", "Home");
            }

            if (UserManager.UserExists(username))
            {
                // パスワードが正しいかどうかをチェック
                if (UserManager.IsPasswordCorrect(username, password))
                {
                    Console.WriteLine("ログイン成功");
                    FormsAuthentication.SetAuthCookie(username, true);
                }
                else
                {
                    Console.WriteLine("パスワードが正しくありません");
                }
            }
            else
            {
                Console.WriteLine("ユーザーが存在しません");
            }

            if (FormsAuthentication.GetRedirectUrl(username, true) != null)
            {
                return Redirect(FormsAuthentication.GetRedirectUrl(username, true));
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}