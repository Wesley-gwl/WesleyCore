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
    /// ����
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// ����
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(options =>
            {
                //options.Filters.Add(typeof(ExceptionResultFilter));//�쳣����
            });
            //�����֤api��֤
            services.AddApiJwtAuthorize((context) =>
            {
                //token����֤����
                // �������context�е�Request��User���Զ���Ȩ����֤������trueΪ���У�����faseʱΪ���أ�����User.Claims���е�¼ʱ�Լ������Claim
                return true;
            }, Configuration);
            services.AddScoped<IGrpcServiceHelper, GrpcServiceHelper>();
            services.AddSwagger();
            //consul
            services.AddConsul(Configuration);
        }

        /// <summary>
        /// ����ע��
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