using ConsulRegister;
using GeekTime.Ordering.API.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Ocelot.JwtAuthorize;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using WesleyCore.Web.Authorizer;
using WesleyCore.Web.Startup.Swagger;
using WesleyPool.Web.Hubs;

namespace WesleyCore
{
    /// <summary>
    /// 启动项
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 配置项
        /// </summary>
        private readonly IConfiguration Configuration;

        /// <summary>
        /// 构造
        /// </summary>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 配置服务
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //允许跨域
            services.AddCors();
#if DEBUG
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("Api", new OpenApiInfo
                {
                    Version = "Api",
                    Title = "Api",
                    Description = "Api文档",
                });

                options.OperationFilter<ApiHeaderToken>();
                var currentAssembly = Assembly.GetExecutingAssembly();
                var xmlDocs = currentAssembly.GetReferencedAssemblies()
                .Union(new AssemblyName[] { currentAssembly.GetName() })
                .Select(a => Path.Combine(Path.GetDirectoryName(currentAssembly.Location), $"{a.Name}.xml"))
                .Where(f => File.Exists(f)).ToArray();
                Array.ForEach(xmlDocs, (d) =>
                {
                    options.IncludeXmlComments(d);//必须加载所有依赖的xml，不然其它程序集的注释不会显示
                });
                //权限按钮参数
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "bearer",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new List<string>()
                    }
                });
                options.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;

                    var versions = methodInfo.DeclaringType
                                               .GetCustomAttributes(true)
                                               .OfType<ApiVersionExtAttribute>()
                                               .Select(attr => attr.Version);

                    return versions.Any(v => v == docName);
                });
            });
#endif
            services.PostConfigure<MvcNewtonsoftJsonOptions>(options =>
            {
                //驼峰序列化方式：命名前几个字母连续大写，驼峰序列化方式 会把连续的大写全部变成小写，直到遇到小写字母和一些分隔字符
                //所以最优的方式首字母小写；如果非要首字母大写，第二个字母请小写
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
            });
            services.AddSingleton(Configuration);
            //新增数据链接
            services.AddSqlServerDomainContext(Configuration["ConnectionStrings:Default"]);
            //添加验证api验证
            services.AddApiJwtAuthorize((context) =>
            {
                //token等验证策略
                // 这里根据context中的Request和User来自定义权限验证，返回true为放行，返回fase时为拦截，其中User.Claims中有登录时自己定义的Claim
                //
                return TokenPermission.ValidatePermission(context);
            }, Configuration);
            services.AddConsul(Configuration);
            //创建推送
            services.AddMediatRServices();
            //创建仓储
            services.AddRepositories();
            //Cap集群
            services.AddEventBus(Configuration);

            services.AddMvc();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint(string.Format("/swagger/{0}/swagger.json", "Api"), "Api");
                });
#endif
            }
            app.UseCors(c => c.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseStaticFiles();

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