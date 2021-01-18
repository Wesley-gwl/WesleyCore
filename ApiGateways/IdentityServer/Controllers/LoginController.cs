using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WesleyCore.User.Proto;

namespace IdentityServer.Controllers
{
    /// <summary>
    ///
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public LoginController(ILoginService.ILoginServiceClient loginServiceClient)
        {
            LoginServiceClient = loginServiceClient;
        }

        public ILoginService.ILoginServiceClient LoginServiceClient;

        /// <summary>
        /// 会员密码登录
        /// </summary>
        /// <param name="dto">登录dto</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> MemberLogin(LoginDto dto)
        {
            var token = await LoginServiceClient.LoginAsync(new LoginForm()
            {
                PhoneNumber = dto.PhoneNumber,
                Password = dto.Password,
                IpAddress = GetUserIp(Request.HttpContext)
            });
            return token.Token;
        }

        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetUserIp(HttpContext context)
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