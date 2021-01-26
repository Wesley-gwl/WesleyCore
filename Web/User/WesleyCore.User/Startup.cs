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
    /// ������
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// ����
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
                options.Filters.Add(typeof(ExceptionResultFilter));//�쳣����
            });
            services.AddSingleton(Configuration);
            //swagger
            services.AddSwagger();
            //����
            RedisClient.RedisCt.InitConnect(Configuration);
            //������������
            services.AddSqlServerDomainContext(Configuration["ConnectionStrings:Default"]);
            //�����֤api��֤
            services.AddApiJwtAuthorize((context) =>
            {
                //token����֤����
                // �������context�е�Request��User���Զ���Ȩ����֤������trueΪ���У�����faseʱΪ���أ�����User.Claims���е�¼ʱ�Լ������Claim
                return true;
            }, Configuration);
            //��������
            services.AddMediatRServices();
            //�����ִ�
            services.AddRepositories();
            //��������
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