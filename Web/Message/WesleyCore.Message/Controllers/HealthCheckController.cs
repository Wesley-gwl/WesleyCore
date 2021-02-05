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
    public class HealthCheckController : ControllerBase
    {
        /// <summary>
        /// 健康检查
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Check() => "ok";
    }
}