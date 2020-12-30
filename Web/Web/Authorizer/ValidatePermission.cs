using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WesleyCore.Web.Controllers;

namespace WesleyCore.Web.Authorizer
{
    /// <summary>
    /// 验证token内api路由权限
    /// </summary>
    public static class TokenPermission
    {
        /// <summary>
        /// 验证规则
        /// </summary>
        /// <returns></returns>
        public static bool ValidatePermission(HttpContext httpContext)
        {
            //var questUrl = httpContext.Request.Path.Value.ToLower();
            ////var roles = httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Role).Value;

            //var claims = httpContext.User.Claims;

            ////验证api权限
            ////通过id从redis里获取权限,如redis里不存在.读取数据库里数据

            //var auth = new AuthModel();
            //httpContext.User.Claims.FirstOrDefault();

            //////先验证路由
            //if (permissions != null && permissions.Where(w => w.Url.Contains("}") ? questUrl.Contains(w.Url.Split('{')[0]) : w.Url.ToLower() == questUrl && w.Predicate.ToLower() == httpContext.Request.Method.ToLower()).Count() > 0)
            //{
            //    var roles = httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Role).Value;
            //    var roleArr = roles.Split(',');
            //    //验证路由名称
            //    var perCount = permissions.Where(w => roleArr.Contains(w.Name)).Count();
            //    if (perCount == 0)
            //    {
            //        httpContext.Response.Headers.Add("error", "no permission");
            //        return false;
            //    }
            //    else
            //    {
            //        return true;
            //    }
            //}
            //else
            //{
            //    return false;
            //}
            return true;
        }
    }
}