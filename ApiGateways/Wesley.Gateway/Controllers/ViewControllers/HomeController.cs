using Microsoft.AspNetCore.Mvc;

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