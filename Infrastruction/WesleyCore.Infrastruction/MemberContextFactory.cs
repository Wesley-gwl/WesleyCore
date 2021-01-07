using DotNetCore.CAP;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using WesleyCore.Configuration;
using WesLeyCore.Const;
using WesleyPool.EntityFrameworkCore;

namespace WesleyCore.EntityFrameworkCore
{
    public class MemberContextFactory : IDesignTimeDbContextFactory<MemberContext>
    {
        protected IMediator _mediator;
        private readonly ICapPublisher _capBus;

        public MemberContextFactory()
        {
        }

        public MemberContextFactory(DbContextOptions options, IMediator mediator, ICapPublisher capBus)
        {
            _mediator = mediator;
            _capBus = capBus;
        }

        /// <summary>
        /// 创建链接
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public MemberContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<MemberContext>();
            var configuration = AppConfigurations.Get(AppContext.BaseDirectory);

            DbContextOptionsConfigurer.Configure(
                builder,
                configuration.GetConnectionString(WesleyCoreConsts.MemberConnectionStringName)
            );
            return new MemberContext(builder.Options, _mediator, _capBus);
        }

        /// <summary>
        /// 创建链接
        /// </summary>
        /// <returns></returns>
        public MemberContext CreateDbContext()
        {
            var builder = new DbContextOptionsBuilder<MemberContext>();
            var configuration = AppConfigurations.Get(AppContext.BaseDirectory);
            DbContextOptionsConfigurer.Configure(
                builder,
                configuration.GetConnectionString(WesleyCoreConsts.MemberConnectionStringName)
            );
            return new MemberContext(builder.Options, _mediator, _capBus);
        }
    }
}