using DotNetCore.CAP;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using WesleyCore.Configuration;
using WesLeyCore.Const;
using WesleyPool.EntityFrameworkCore;

namespace WesleyCore.EntityFrameworkCore
{
    public class DomainContextFactory : IDesignTimeDbContextFactory<DomainContext>
    {
        protected IMediator _mediator;
        private readonly ICapPublisher _capBus;

        public DomainContextFactory()
        {
        }

        public DomainContextFactory(DbContextOptions options, IMediator mediator, ICapPublisher capBus)
        {
            _mediator = mediator;
            _capBus = capBus;
        }

        /// <summary>
        /// 创建链接
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public DomainContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<DomainContext>();
            var configuration = AppConfigurations.Get(AppContext.BaseDirectory);

            DbContextOptionsConfigurer.Configure(
                builder,
                configuration.GetConnectionString(WesleyCoreConsts.ConnectionStringName)
            );
            return new DomainContext(builder.Options, _mediator, _capBus);
        }

        /// <summary>
        /// 创建链接
        /// </summary>
        /// <returns></returns>
        public DomainContext CreateDbContext()
        {
            var builder = new DbContextOptionsBuilder<DomainContext>();
            var configuration = AppConfigurations.Get(AppContext.BaseDirectory);
            DbContextOptionsConfigurer.Configure(
                builder,
                configuration.GetConnectionString(WesleyCoreConsts.ConnectionStringName)
            );
            return new DomainContext(builder.Options, _mediator, _capBus);
        }
    }
}