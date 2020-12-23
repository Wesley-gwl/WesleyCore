using Microsoft.AspNetCore.Mvc;
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
    public class WesleyCoreAPIBaseController : WesleyCoreControllerBase
    {
    }
}