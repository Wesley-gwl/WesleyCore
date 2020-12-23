using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WesleyCore.Web.Controllers;

namespace Web.Controllers.ViewControllers
{
    /// <summary>
    /// 主页控制器
    /// </summary>
    public class HomeController : WesleyCoreControllerBase
    {
        /// <summary>
        /// 登入
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View("~/Views/System/Home/Login.cshtml");
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View("~/Views/System/Home/Index.cshtml");
        }
    }
}