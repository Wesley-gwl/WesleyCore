using GeekTime.Ordering.API.Application.IntegrationEvents;
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
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using WesleyCore.Web.Authorizer;
using WesleyCore.Web.Startup.Swagger;
using WesleyPool.Web.Hubs;

namespace WesleyCore
{
    /// <summary>
    /// ������
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// ������
        /// </summary>
        private readonly IConfiguration Configuration;

        /// <summary>
        /// ����
        /// </summary>
        public Startup(IWebHostEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables().Build();
            JWTUtil.key = Configuration["Customization:JwtKey"];
        }

        /// <summary>
        /// ���÷���
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddControllersWithViews(options =>
            //{
            //    //options.Filters.Add(typeof(AuthorFilterAttribute)); //��ݹ�����
            //    //options.Filters.Add(typeof(ExceptionResultFilter));//�쳣����
            //}).AddNewtonsoftJson();

            //�������
            services.AddCors();
            services.AddSignalR();
#if DEBUG
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("Api", new OpenApiInfo
                {
                    Version = "Api",
                    Title = "Api",
                    Description = "Api�ĵ�",
                });

                //options.OperationFilter<ApiHeaderToken>();
                //var currentAssembly = Assembly.GetExecutingAssembly();
                //var xmlDocs = currentAssembly.GetReferencedAssemblies()
                //.Union(new AssemblyName[] { currentAssembly.GetName() })
                //.Select(a => Path.Combine(Path.GetDirectoryName(currentAssembly.Location), $"{a.Name}.xml"))
                //.Where(f => File.Exists(f)).ToArray();
                //Array.ForEach(xmlDocs, (d) =>
                //{
                //    options.IncludeXmlComments(d);//�����������������xml����Ȼ�������򼯵�ע�Ͳ�����ʾ
                //});
                //options.DocInclusionPredicate((docName, apiDesc) =>
                //{
                //    if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;

                //    var versions = methodInfo.DeclaringType
                //                               .GetCustomAttributes(true)
                //                               .OfType<ApiVersionExtAttribute>()
                //                               .Select(attr => attr.Version);

                //    return versions.Any(v => v == docName);
                //});
            });
#endif
            services.PostConfigure<MvcNewtonsoftJsonOptions>(options =>
            {
                //�շ����л���ʽ������ǰ������ĸ������д���շ����л���ʽ ��������Ĵ�дȫ�����Сд��ֱ������Сд��ĸ��һЩ�ָ��ַ�
                //�������ŵķ�ʽ����ĸСд�������Ҫ����ĸ��д���ڶ�����ĸ��Сд
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
            });
            services.AddSingleton(Configuration);

            //������������
            services.AddSqlServerDomainContext(Configuration["ConnectionStrings:Default"]);
            //��������
            services.AddMediatRServices();
            //�����ִ�
            services.AddRepositories();
            //Cap��Ⱥ
            services.AddEventBus(Configuration);

            services.Configure<IISOptions>(options =>
            {
                options.ForwardClientCertificate = false;
            });
            services.AddMvc().AddRazorRuntimeCompilation();
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
            }
            else
            {
                app.UseExceptionHandler("/Home/Login");
            }
#if DEBUG
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(string.Format("/swagger/{0}/swagger.json", "Api"), "Api");
            });
#endif
            app.UseCors(c => c.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseWebSockets();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Login}/{id?}");//·�ɹ���
                endpoints.MapHub<SystemHub>("/systemHub");//����
            });
        }
    }
}