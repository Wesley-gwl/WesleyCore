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
    /// ������
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// �����ļ�
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// ����ע��
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //�������
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
                    Description = "Api�ĵ�",
                });
                options.OperationFilter<ApiHeaderToken>();
                var currentAssembly = Assembly.GetExecutingAssembly();
                var xmlDocs = currentAssembly.GetReferencedAssemblies()
                .Union(new AssemblyName[] { currentAssembly.GetName() })
                .Select(a => Path.Combine(Path.GetDirectoryName(currentAssembly.Location), $"{a.Name}.xml"))
                .Where(f => File.Exists(f)).ToArray();
                Array.ForEach(xmlDocs, (d) =>
                {
                    options.IncludeXmlComments(d);//�����������������xml����Ȼ�������򼯵�ע�Ͳ�����ʾ
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
            //    //�������context�е�Request��User���Զ���Ȩ����֤������trueΪ���У�����faseʱΪ���أ�����User.Claims���е�¼ʱ�Լ������Claim
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
        /// �м��ע��
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