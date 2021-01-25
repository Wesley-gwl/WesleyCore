using Grpc.Core;
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

namespace OrderAggregation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OrderAggregation", Version = "v1" });
            });
            //services.AddGrpcClient<ILoginService.ILoginServiceClient>(option =>
            //{
            //    option.Address = new Uri("https://localhost:5003");
            //}).ConfigureChannel((gRpcOption) =>
            //{
            //    //ssl TSL,֤������ϼ���token
            //    gRpcOption.Credentials = ChannelCredentials.Create(
            //        new SslCredentials(),
            //        CallCredentials.FromInterceptor(async (context, metadata) =>
            //        {
            //            //context ֤��������
            //            //metadata ����ͷ����
            //            //��λ�ȡtoken
            //            //var httpclient = new HttpClient();
            //            //httpclient.DefaultRequestVersion = new Version(2, 0);
            //            //string address = "localhost:5002";
            //            var token = "123";
            //            metadata.Add("Authorization", $"Bearer {token}");

            //        })
            //        );
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OrderAggregation v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}