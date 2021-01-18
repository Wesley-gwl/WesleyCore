using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.JwtAuthorize;
using System;
using System.Reflection;
using WesleyCore.Domin.Abstractions;
using WesleyCore.EntityFrameworkCore;
using WesleyCore.Infrastruction.Core;
using WesleyCore.Infrastructure;
using WesleyCore.User;
using WesleyCore.User.Application.Queries.User;

namespace WesleyCore
{
    /// <summary>
    /// 中间件注册
    /// </summary>
    public static class ServiceCollectionExtensions
    {
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