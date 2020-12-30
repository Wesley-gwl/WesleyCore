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
using WesleyCore.Application.Queries.System.User.Dto;
using WesleyCore.Web.Startup.Swagger;
using WesleyUntity;

namespace WesleyCore.Web.Controllers.ApiControllers
{
    /// <summary>
    /// 登录
    /// </summary>
    [ApiController]
    [Route("/api/[controller]/[action]")]
    [ApiVersionExt("Api")]
    public class LoginController : WesleyCoreControllerBase
    {
        private IMediator _mediator;
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
            // var re = await _mediator.Send(input, HttpContext.RequestAborted);
            var claims = new Claim[] {
                        new Claim(ClaimTypes.Name, "帅哥"),
                        new Claim(ClaimTypes.Role, "Role"),//权限
                        new Claim(ClaimTypes.Sid ,"18868806417"),
                        new Claim ("Ip",GetUserIp(HttpContext)),
                        new Claim("TenantId","0")//租户
                    };
            var token = JsonConvert.SerializeObject(_tokenBuilder.BuildJwtToken(claims, DateTime.UtcNow, DateTime.Now.AddMinutes(2)));
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