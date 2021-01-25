using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WesleyCore;
using Ocelot.JwtAuthorize;
using Wesley.GrpcService;
using ConsulRegister;

namespace UserAggregation
{
    /// <summary>
    /// 启动
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 配置
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 服务发现
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(options =>
            {
                //options.Filters.Add(typeof(ExceptionResultFilter));//异常过滤
            });
            //添加验证api验证
            services.AddApiJwtAuthorize((context) =>
            {
                //token等验证策略
                // 这里根据context中的Request和User来自定义权限验证，返回true为放行，返回fase时为拦截，其中User.Claims中有登录时自己定义的Claim
                return true;
            }, Configuration);
            services.AddScoped<IGrpcServiceHelper, GrpcServiceHelper>();
            services.AddSwagger();
            //consul
            services.AddConsul(Configuration);
        }

        /// <summary>
        /// 服务注册
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint(string.Format("/swagger/{0}/swagger.json", "Api"), "Api"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseConsul(Configuration);
        }
    }
}