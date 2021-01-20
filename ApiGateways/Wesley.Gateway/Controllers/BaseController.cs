using Microsoft.AspNetCore.Mvc;
using WeslesyCore.Web.Startup.Swagger;

namespace Wesley.Gateway.Controllers
{
    /// <summary>
    /// 控制器积累
    /// </summary>
    public class BaseController : Controller
    {
    }

    /// <summary>
    /// api积累控制器
    /// </summary>
    [ApiController]
    [Route("/api/[controller]/[action]")]
    [ApiVersionExt("Api")]
    public class ApiBaseController : BaseController
    {
    }
}