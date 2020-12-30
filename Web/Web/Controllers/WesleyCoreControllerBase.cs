using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WesleyCore.Web.Startup.Swagger;

namespace WesleyCore.Web.Controllers
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    public class WesleyCoreControllerBase : Controller
    {
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