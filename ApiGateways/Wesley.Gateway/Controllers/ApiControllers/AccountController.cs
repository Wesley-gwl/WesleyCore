using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Wesley.Gateway.Controllers;
using WesleyPC.Gateway;

namespace GeekTime.Mobile.Gateway.Controllers
{
    /// <summary>
    /// 用户登录
    /// </summary>
    public class AccountController : ApiBaseController
    {
        /// <summary>
        /// cookie登入
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CookieLogin(string userName, string passWord)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);//一定要声明AuthenticationScheme
            identity.AddClaim(new Claim("Name", userName));
            await this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
            return Content("login");
        }

        /// <summary>
        /// JWT登入
        /// </summary>
        /// <param name="securityKey"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> JwtLogin([FromServices] SymmetricSecurityKey securityKey, string userName)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("Name", userName));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: "localhost",//签发者
                audience: "localhost",//客户端
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);
            var t = new JwtSecurityTokenHandler().WriteToken(token);
            return Content(t);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> SetRedis(string key, string text)
        {
            RedisClient.redisClient.SetStringKey<string>(key, text);

            return Content(text);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetRedis(string text)
        {
            var re = RedisClient.redisClient.GetStringKey(text);
            return Content(re);
        }
    }
}