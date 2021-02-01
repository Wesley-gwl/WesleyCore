using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace WesleyCore.Web.Controllers.ApiControllers
{
    /// <summary>
    /// 健康检查
    /// </summary>
    [Route("/api/[controller]/[action]")]
    [ApiController]
    public class HealthCheck : ControllerBase
    {
        /// <summary>
        /// 健康检查
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Check()
        {
            //心跳,consul会每隔几秒调一次
            Console.WriteLine($"心跳检测");
            return Ok("成功");
        }
    }
}