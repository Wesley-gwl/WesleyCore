using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WesleyCore.Web.Startup.Swagger;
using WesleyUntity;

namespace WesleyCore.Web.Controllers
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    public class WesleyCoreControllerBase : Controller
    {
        /// <summary>
        /// 当前用户信息
        /// </summary>
        protected AuthModel CurrentOperator
        {
            get
            {
                var claims = HttpContext.User.Claims;
                return new AuthModel
                {
                    UserId = claims.SingleOrDefault(p => p.Type == ClaimTypes.Sid).Value.ToGuid().Value,
                    UserName = claims.SingleOrDefault(p => p.Type == ClaimTypes.Name)?.Value,
                    PhoneNumber = claims.SingleOrDefault(p => p.Type == ClaimTypes.MobilePhone)?.Value,
                    TenantID = claims.SingleOrDefault(p => p.Type == "TenantId").ToInt(0).Value,
                };
            }
        }
    }

    /// <summary>
    /// Api
    /// </summary>
    [ApiController]
    [Route("/api/[controller]/[action]")]
    [ApiVersionExt("Api")]
    [Authorize("permission")]//token验证
    public class WesleyCoreAPIBaseController : WesleyCoreControllerBase
    {
    }
}