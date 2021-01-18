using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Ocelot.JwtAuthorize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WesleyCore.Application;
using WesleyCore.Web;
using WesleyUntity;

namespace WesleyCore.Login.Controllers
{
    /// <summary>
    /// 登录
    /// </summary>
    [Route("/api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration Configuration;
        private readonly ITokenBuilder _tokenBuilder;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="configuration"></param>
        /// <param name="tokenBuilder"></param>
        public LoginController(IMediator mediator, IConfiguration configuration, ITokenBuilder tokenBuilder)
        {
            _mediator = mediator;
            _tokenBuilder = tokenBuilder;
            Configuration = configuration;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BizResult<string>> Login(LoginDto input)
        {
            var hash = Configuration["Customization:PwdKey"];
            input.Password = EncryptUtil.AESEncrypt(input.Password, hash);
            var expired = DateTime.Now.AddMinutes(10);
            // var re = await _mediator.Send(input, HttpContext.RequestAborted);
            var claims = new Claim[] {
                        new Claim(ClaimTypes.Name, "帅哥"),
                        new Claim(ClaimTypes.Sid ,Guid.NewGuid().ToString()),
                        new Claim ("Ip",GetUserIp(HttpContext)),
                        new Claim("TenantId","0"),//租户
                        new Claim(ClaimTypes.MobilePhone,"18868806417")
                    };
            var token = JsonConvert.SerializeObject(_tokenBuilder.BuildJwtToken(claims, DateTime.UtcNow, expired));
            return new BizResult<string>(token);
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