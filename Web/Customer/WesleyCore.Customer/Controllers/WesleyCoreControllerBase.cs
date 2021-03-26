﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using WesleyUntity;

namespace WesleyCore.Controllers
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    public class WesleyCoreControllerBase : ControllerBase
    {
        /// <summary>
        /// 当前用户信息
        /// </summary>
        protected AuthModel Member { get; private set; }

        /// <summary>
        /// 当前用户信息方法
        /// </summary>
        protected void GetUser()
        {
            var claims = HttpContext.User.Claims;
            Member.UserId = claims.SingleOrDefault(p => p.Type == ClaimTypes.Sid).Value.ToGuid().Value;
            Member.UserName = claims.SingleOrDefault(p => p.Type == ClaimTypes.Name)?.Value;
            Member.PhoneNumber = claims.SingleOrDefault(p => p.Type == ClaimTypes.MobilePhone)?.Value;
            Member.TenantID = claims.SingleOrDefault(p => p.Type == "TenantId").ToInt(0).Value;
        }
    }

    /// <summary>
    /// Api
    /// </summary>
    [ApiController]
    [Route("/api/[controller]/[action]")]
    [Authorize("permission")]//token验证
    public class WesleyCoreAPIBaseController : WesleyCoreControllerBase
    {
    }
}