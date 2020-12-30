using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Wesley.Gateway.Controllers
{
    /// <summary>
    /// 主页
    /// </summary>
    public class HomeController : BaseController
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }
    }
}