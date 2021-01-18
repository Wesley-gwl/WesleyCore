using DotNetCore.CAP;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using WesleyCore.Login.Infrastructure;

namespace WesleyCore.EntityFrameworkCore
{
    public class LoginContextFactory : IDesignTimeDbContextFactory<LoginContext>
    {
        protected IMediator _mediator;
        private readonly ICapPublisher _capBus;

        public LoginContextFactory()
        {
        }

        public LoginContextFactory(DbContextOptions options, IMediator mediator, ICapPublisher capBus)
        {
            _mediator = mediator;
            _capBus = capBus;
        }

        /// <summary>
        /// 创建链接
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public LoginContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<LoginContext>();
            var configuration = AppConfigurations.Get(AppContext.BaseDirectory);

            DbContextOptionsConfigurer.Configure(
                builder,
                configuration.GetConnectionString("Default")
            );
            return new LoginContext(builder.Options, _mediator, _capBus);
        }

        /// <summary>
        /// 创建链接
        /// </summary>
        /// <returns></returns>
        public LoginContext CreateDbContext()
        {
            var builder = new DbContextOptionsBuilder<LoginContext>();
            var configuration = AppConfigurations.Get(AppContext.BaseDirectory);
            DbContextOptionsConfigurer.Configure(
                builder,
                configuration.GetConnectionString("Default")
            );
            return new LoginContext(builder.Options, _mediator, _capBus);
        }
    }
}