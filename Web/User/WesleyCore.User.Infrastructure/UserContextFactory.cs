using DotNetCore.CAP;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using WesleyCore.Infrastructure;

namespace WesleyCore.EntityFrameworkCore
{
    public class UserContextFactory : IDesignTimeDbContextFactory<UserContext>
    {
        protected IMediator _mediator;
        private readonly ICapPublisher _capBus;
        // private ITenantProvider _tenantProvider;

        public UserContextFactory()
        {
        }

        public UserContextFactory(DbContextOptions options, IMediator mediator, ICapPublisher capBus)
        {
            _mediator = mediator;
            _capBus = capBus;
        }

        /// <summary>
        /// 创建链接
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public UserContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<UserContext>();
            var configuration = AppConfigurations.Get(AppContext.BaseDirectory);

            DbContextOptionsConfigurer.Configure(
                builder,
                configuration.GetConnectionString("Default")
            );
            return new UserContext(builder.Options, _mediator, _capBus);
        }

        /// <summary>
        /// 创建链接
        /// </summary>
        /// <returns></returns>
        public UserContext CreateDbContext()
        {
            var builder = new DbContextOptionsBuilder<UserContext>();
            var configuration = AppConfigurations.Get(AppContext.BaseDirectory);
            DbContextOptionsConfigurer.Configure(
                builder,
                configuration.GetConnectionString("Default")
            );
            return new UserContext(builder.Options, _mediator, _capBus);
        }
    }
}