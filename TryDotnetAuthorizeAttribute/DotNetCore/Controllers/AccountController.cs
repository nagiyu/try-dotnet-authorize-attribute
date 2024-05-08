using CommonAuthService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace DotNetFramework.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return new ContentResult { Content = "ユーザー名またはパスワードが入力されていません", StatusCode = StatusCodes.Status400BadRequest };
            }

            if (UserManager.UserExists(username))
            {
                // パスワードが正しいかどうかをチェック
                if (UserManager.IsPasswordCorrect(username, password))
                {
                    var claims = new List<Claim>
                    {
                        new(ClaimTypes.Name, username)
                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties();

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme, 
                        new ClaimsPrincipal(claimsIdentity), 
                        authProperties);
                }
                else
                {
                    return new ContentResult { Content = "パスワードが正しくありません", StatusCode = StatusCodes.Status401Unauthorized };
                }
            }
            else
            {
                return new ContentResult { Content = "ユーザーが存在しません", StatusCode = StatusCodes.Status404NotFound };
            }

            return new ContentResult { Content = "ログインしました", StatusCode = StatusCodes.Status200OK };
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return new ContentResult { Content = "ログアウトしました", StatusCode = StatusCodes.Status200OK };
        }
    }
}