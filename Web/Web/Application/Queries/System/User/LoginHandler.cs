using MediatR;
using Microsoft.AspNetCore.Http;
using Ocelot.JwtAuthorize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WesleyCore.Application.Queries.System.User.Dto;
using Newtonsoft.Json;
using static Ocelot.JwtAuthorize.TokenBuilder;

namespace WesleyCore.Application.Queries
{
    /// <summary>
    /// 登入
    /// </summary>
    public class LoginHandler : IRequestHandler<LoginDto, string>
    {
        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<string> Handle(LoginDto request, CancellationToken cancellationToken)
        {
            return "123";
            //var claims = new Claim[] {
            //            new Claim(ClaimTypes.Name, "帅哥"),
            //            new Claim(ClaimTypes.Role, "Role"),//权限
            //            new Claim(ClaimTypes.Sid ,"18868806417"),
            //            new Claim ("Ip",request.IpAddress),
            //            new Claim("TenantId","0")//租户
            //        };
            //return Task.FromResult(JsonConvert.SerializeObject(_tokenBuilder.BuildJwtToken(claims, DateTime.UtcNow, DateTime.Now.AddMinutes(2), TokenType.PC)));
        }
    }
}