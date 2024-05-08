using DotNetFramework.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetFramework.Controllers
{
    public class SampleController : Controller
    {
        public ActionResult NoAttribute()
        {
            return Content("NoAttribute");
        }

        [AllowAnonymous]
        public ActionResult AllowAnonymous()
        {
            return Content("AllowAnonymousAttribute");
        }

        [CustomAuthorize]
        public ActionResult Content()
        {
            return Content("CustomAuthorizeAttribute");
        }

        [Authorize]
        public ActionResult Authorize()
        {
            return Content("AuthorizeAttribute");
        }
    }
}