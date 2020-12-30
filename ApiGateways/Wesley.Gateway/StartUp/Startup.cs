using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.JwtAuthorize;
using Ocelot.Middleware;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using WeslesyCore.Web.Startup.Swagger;
using Wesley.Web.Authorizer;

namespace WesleyPC.Gateway
{
    /// <summary>
    /// 启动项
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 启动构造
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 配置文件
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 服务注册
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //允许跨域
            services.AddCors();
            services.AddSignalR();
            services.AddSingleton(Configuration);
            RedisClient.redisClient.InitConnect(Configuration);
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
            services.AddOcelotJwtAuthorize(Configuration);
            services.AddOcelot(Configuration);
            //services.AddApiJwtAuthorize((context) =>
            //{
            //    //validate permissions return(permit) true or false(denied) API Controller, "permission" is PolicyName of appsettion.json
            //    //这里根据context中的Request和User来自定义权限验证，返回true为放行，返回fase时为拦截，其中User.Claims中有登录时自己定义的Claim
            //    return true;
            //}, Configuration);
            services.Configure<IISOptions>(options =>
            {
                options.ForwardClientCertificate = false;
            });
            services.AddControllersWithViews();
            services.AddMvc();
        }

        /// <summary>
        /// 中间件注册
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Login");
                app.UseHsts();
            }
            app.UseWebSockets();
            app.UseCors(c => c.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseStaticFiles();
#if DEBUG
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(string.Format("/swagger/{0}/swagger.json", "Api"), "Api");
            });
#endif
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Login}/{id?}");
            });
            app.UseOcelot();
        }
    }
}