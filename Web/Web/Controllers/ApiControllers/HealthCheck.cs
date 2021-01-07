using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WesleyCore.Web.Startup.Swagger;

namespace WesleyCore.Web.Controllers.ApiControllers
{
    /// <summary>
    /// 健康检查
    /// </summary>
    [Route("/api/[controller]/[action]")]
    [ApiController]
    [ApiVersionExt("Api")]
    public class HealthCheck : ControllerBase
    {
        /// <summary>
        /// 创建订单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Check()
        {
            //心跳,consul会每隔几秒调一次
            Console.WriteLine($"web Invoke");
            return Ok();
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Check2()
        {
            //心跳,consul会每隔几秒调一次
            Console.WriteLine($"web Invoke2");
            return Ok("123123");
        }
    }
}