using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Ocelot.JwtAuthorize;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using WesleyCore.Domin.Abstractions;
using WesleyCore.EntityFrameworkCore;
using WesleyCore.Infrastruction.Core;
using WesleyCore.Infrastructure;
using WesleyCore.User;
using WesleyCore.User.Domain;
using WesleyCore.User.Infrastructure;
using WesleyCore.User.Infrastructure.Repositories;

namespace WesleyCore
{
    /// <summary>
    /// 中间件注册
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加swagger
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("Api", new OpenApiInfo
                {
                    Version = "Api",
                    Title = "Api",
                    Description = "Api文档",
                });

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
            });
            return services;
        }

        /// <summary>
        /// mediatR中间件注册
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMediatRServices(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UserContextTransactionBehavior<,>));

            services.AddMediatR(typeof(Program).Assembly
                //typeof(LoginHandler).Assembly
                );
            //services.AddMediatR( Assembly.GetExecutingAssembly());
            //services.AddMediatR(
            //    typeof(CreateOrderCommandHandler).Assembly,
            //    typeof(OrderCreatedDomainEventHandler).Assembly,
            //    typeof(LoginHandler).Assembly);
            return services;
        }

        /// <summary>
        /// 新增数据库
        /// </summary>
        /// <param name="services"></param>
        /// <param name="optionsAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddDomainContext(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction)
        {
            return services.AddDbContext<UserContext>(optionsAction);
        }

        /// <summary>
        /// 新增数据库链接
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static IServiceCollection AddSqlServerDomainContext(this IServiceCollection services, string connectionString)
        {
            return services.AddDomainContext(builder =>
            {
                builder.UseSqlServer(connectionString);
            });
        }

        /// <summary>
        /// 创建仓储
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            //获取租户id
            services.AddHttpContextAccessor();
            services.AddScoped<ITenantProvider, TenantProvider>();
            services.AddScoped<ITokenBuilder, TokenBuilder>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMemberRepository, MemberRepository>();
            return services;
        }

        /// <summary>
        /// 新增订阅配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IEventBus, EventBus>();
            services.AddCap(options =>
            {
                options.UseSqlServer(configuration["ConnectionStrings:Default"]); // SQL Server
                options.UseRabbitMQ(options =>
                {
                    configuration.GetSection("RabbitMQ").Bind(options);
                });
                //options.UseDashboard();
            });

            return services;
        }
    }
}