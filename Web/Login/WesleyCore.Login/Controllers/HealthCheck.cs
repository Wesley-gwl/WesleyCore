using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}