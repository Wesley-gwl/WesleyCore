using DotNetCore.CAP;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using WesleyCore.Infrastructure;

namespace WesleyCore.EntityFrameworkCore
{
    public class CustomerContextFactory : IDesignTimeDbContextFactory<CustomerContext>
    {
        protected IMediator _mediator;
        private readonly ICapPublisher _capBus;
        // private ITenantProvider _tenantProvider;

        public CustomerContextFactory()
        {
        }

        public CustomerContextFactory(DbContextOptions options, IMediator mediator, ICapPublisher capBus)
        {
            _mediator = mediator;
            _capBus = capBus;
        }

        /// <summary>
        /// 创建链接
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public CustomerContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<CustomerContext>();
            var configuration = AppConfigurations.Get(AppContext.BaseDirectory);

            DbContextOptionsConfigurer.Configure(
                builder,
                configuration.GetConnectionString("Default")
            );
            return new CustomerContext(builder.Options, _mediator, _capBus);
        }

        /// <summary>
        /// 创建链接
        /// </summary>
        /// <returns></returns>
        public CustomerContext CreateDbContext()
        {
            var builder = new DbContextOptionsBuilder<CustomerContext>();
            var configuration = AppConfigurations.Get(AppContext.BaseDirectory);
            DbContextOptionsConfigurer.Configure(
                builder,
                configuration.GetConnectionString("Default")
            );
            return new CustomerContext(builder.Options, _mediator, _capBus);
        }
    }
}