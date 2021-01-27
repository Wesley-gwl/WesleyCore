using IdentityServer.GrpcService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WesleyCore.User.Proto;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Controllers
{
    /// <summary>
    ///
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        /// <summary>
        /// 构造
        /// </summary>
        public LoginController(IGrpcServiceHelper grpcServiceHelper)
        {
            this.grpcServiceHelper = grpcServiceHelper;
        }

        /// <summary>
        ///
        /// </summary>
        private readonly IGrpcServiceHelper grpcServiceHelper;

        /// <summary>
        /// 会员密码登录
        /// </summary>
        /// <param name="dto">登录dto</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> MemberLogin(LoginDto dto)
        {
            var loginServiceClient = await grpcServiceHelper.GetLoginService();
            var token = await loginServiceClient.LoginAsync(new LoginForm()
            {
                PhoneNumber = dto.PhoneNumber,
                Password = dto.Password,
                IpAddress = GetUserIp(Request.HttpContext)
            }, cancellationToken: HttpContext.RequestAborted);
            return token.Token;
        }

        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static string GetUserIp(HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(ip))
            {
                ip = context.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }
    }
}