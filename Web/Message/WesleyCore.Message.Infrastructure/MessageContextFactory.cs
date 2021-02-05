using DotNetCore.CAP;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using WesleyCore.Infrastructure;

namespace WesleyCore.EntityFrameworkCore
{
    public class MessageContextFactory : IDesignTimeDbContextFactory<MessageContext>
    {
        protected IMediator _mediator;
        private readonly ICapPublisher _capBus;
        // private ITenantProvider _tenantProvider;

        public MessageContextFactory()
        {
        }

        public MessageContextFactory(DbContextOptions options, IMediator mediator, ICapPublisher capBus)
        {
            _mediator = mediator;
            _capBus = capBus;
        }

        /// <summary>
        /// 创建链接
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public MessageContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<MessageContext>();
            var configuration = AppConfigurations.Get(AppContext.BaseDirectory);

            DbContextOptionsConfigurer.Configure(
                builder,
                configuration.GetConnectionString("Default")
            );
            return new MessageContext(builder.Options, _mediator, _capBus);
        }

        /// <summary>
        /// 创建链接
        /// </summary>
        /// <returns></returns>
        public MessageContext CreateDbContext()
        {
            var builder = new DbContextOptionsBuilder<MessageContext>();
            var configuration = AppConfigurations.Get(AppContext.BaseDirectory);
            DbContextOptionsConfigurer.Configure(
                builder,
                configuration.GetConnectionString("Default")
            );
            return new MessageContext(builder.Options, _mediator, _capBus);
        }
    }
}