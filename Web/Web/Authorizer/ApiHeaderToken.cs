using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace WesleyCore.Web.Authorizer
{
    /// <summary>
    /// swigger token
    /// </summary>
    public class ApiHeaderToken : IOperationFilter
    {
        /// <summary>
        /// 在swagger接口文档参数中加入token
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            //是否定义匿名访问
            //var ac = (context.ApiDescription.ActionDescriptor as ControllerActionDescriptor).MethodInfo.IsDefined(typeof(AllowAnonymousAttribute), true);
            var ac = (context.ApiDescription.ActionDescriptor as ControllerActionDescriptor).MethodInfo;
            var ctl = ac.ReflectedType;
            if (!(ac.IsDefined(typeof(AllowAnonymousAttribute), true) || ctl.IsDefined(typeof(AllowAnonymousAttribute), true)))
            {
                if (operation.Parameters == null)
                {
                    operation.Parameters = new List<OpenApiParameter>();
                }

                operation.Parameters.Insert(0, new OpenApiParameter()
                {
                    Name = "token",
                    Description = "JWT",
                    In = ParameterLocation.Header,
                    Required = false
                });
            }
        }
    }
}