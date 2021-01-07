using GeekTime.Ordering.API.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Ocelot.JwtAuthorize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WesleyCore.Login
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
        ///
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
#if DEBUG
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WesleyCore.Login", Version = "v1" });
            });
#endif
            services.AddSingleton(Configuration);
            //������������
            services.AddSqlServerDomainContext(Configuration["ConnectionStrings:Default"]);
            services.AddTokenJwtAuthorize(Configuration);
            //��������
            services.AddMediatRServices();
            //�����ִ�
            services.AddRepositories();
            services.AddEventBus(Configuration);
            services.Configure<IISOptions>(options =>
            {
                options.ForwardClientCertificate = false;
            });
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
#if DEBUG
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WesleyCore.Login v1"));
#endif
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}