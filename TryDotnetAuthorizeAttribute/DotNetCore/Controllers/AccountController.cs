using CommonAuthService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Reflection;

namespace DotNetFramework.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public class LoginModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        //[HttpPost]
        //public async Task<IActionResult> Login(string username, string password)
        //{
        //    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        //    {
        //        return new ContentResult { Content = "ユーザー名またはパスワードが入力されていません", StatusCode = StatusCodes.Status400BadRequest };
        //    }

        //    var model = new LoginModel { Username = username, Password = password };

        //    return await AuthenticateUser(model);
        //}

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                return new ContentResult { Content = "ユーザー名またはパスワードが入力されていません", StatusCode = StatusCodes.Status400BadRequest };
            }

            return await AuthenticateUser(model);
        }

        private async Task<IActionResult> AuthenticateUser(LoginModel model)
        {
            if (UserManager.UserExists(model.Username))
            {
                // パスワードが正しいかどうかをチェック
                if (UserManager.IsPasswordCorrect(model.Username, model.Password))
                {
                    await SignInUser(model.Username);
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

        private async Task SignInUser(string username)
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

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return new ContentResult { Content = "ログアウトしました", StatusCode = StatusCodes.Status200OK };
        }
    }
}