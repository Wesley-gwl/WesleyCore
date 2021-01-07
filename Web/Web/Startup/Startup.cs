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
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// ���÷���
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //�������
            services.AddCors();
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
                //Ȩ�ް�ť����
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
                //�շ����л���ʽ������ǰ������ĸ������д���շ����л���ʽ ��������Ĵ�дȫ�����Сд��ֱ������Сд��ĸ��һЩ�ָ��ַ�
                //�������ŵķ�ʽ����ĸСд�������Ҫ����ĸ��д���ڶ�����ĸ��Сд
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
            });
            services.AddSingleton(Configuration);
            //������������
            services.AddSqlServerDomainContext(Configuration["ConnectionStrings:Default"]);
            //�����֤api��֤
            services.AddApiJwtAuthorize((context) =>
            {
                //token����֤����
                // �������context�е�Request��User���Զ���Ȩ����֤������trueΪ���У�����faseʱΪ���أ�����User.Claims���е�¼ʱ�Լ������Claim
                //
                return TokenPermission.ValidatePermission(context);
            }, Configuration);
            services.AddConsul(Configuration);
            //��������
            services.AddMediatRServices();
            //�����ִ�
            services.AddRepositories();
            //Cap��Ⱥ
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