using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using WesleyCore.Domin.Abstractions;
using WesleyCore.EntityFrameworkCore;
using WesleyCore.Infrastruction.Core;
using WesleyCore.Infrastruction.Repositories;
using WesleyCore.Login;

namespace GeekTime.Ordering.API.Extensions
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
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MemberContextTransactionBehavior<,>));
            services.AddMediatR(
                typeof(Program).Assembly);
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
            return services.AddDbContext<MemberContext>(optionsAction);
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
            //services.AddScoped<IOrderRepository, OrderRepository>();
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