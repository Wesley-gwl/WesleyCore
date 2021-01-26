using ConsulRegister;
using log4net;
using log4net.Config;
using log4net.Repository;
using log4net.Repository.Hierarchy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Ocelot.JwtAuthorize;
using System.IO;
using Wesley.Filter;
using WesleyCore.User.Domain;
using WesleyCore.User.GrpcService;
using WesleyRedis;
using WesleyUntity;

namespace WesleyCore.User
{
    /// <summary>
    /// 启动项
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 配置
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(typeof(ExceptionResultFilter));//异常过滤
            });
            services.AddSingleton(Configuration);
            //swagger
            services.AddSwagger();
            //缓存
            RedisClient.RedisCt.InitConnect(Configuration);
            //新增数据链接
            services.AddSqlServerDomainContext(Configuration["ConnectionStrings:Default"]);
            //添加验证api验证
            services.AddApiJwtAuthorize((context) =>
            {
                //token等验证策略
                // 这里根据context中的Request和User来自定义权限验证，返回true为放行，返回fase时为拦截，其中User.Claims中有登录时自己定义的Claim
                return true;
            }, Configuration);
            //创建推送
            services.AddMediatRServices();
            //创建仓储
            services.AddRepositories();
            //订阅新增
            services.AddEventBus(Configuration);
            //AutoMap
            services.AddAutoMap();
            //Grpc
            services.AddGrpc();
            //consul
            services.AddConsul(Configuration);
        }

        /// <summary>
        ///
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
                #region GrpcService

                endpoints.MapGrpcService<LoginService>();
                endpoints.MapGrpcService<UserService>();

                #endregion GrpcService

                endpoints.MapControllers();
            });
            app.UseConsul(Configuration);
        }
    }
}