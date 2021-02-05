using DotNetCore.CAP;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Toolbelt.ComponentModel.DataAnnotations;
using WesleyCore.Domin.Abstractions;
using WesleyCore.Infrastructure.Core;

namespace WesleyCore.Infrastructure
{
    /// <summary>
    /// 会员微服务数据链接
    /// </summary>
    public class MessageContext : EFContext
    {
        private readonly int _tenantId;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="options"></param>
        /// <param name="mediator"></param>
        /// <param name="capBus"></param>
        public MessageContext(DbContextOptions options, IMediator mediator, ICapPublisher capBus, ITenantProvider tenantProvider) : base(options, mediator, capBus)
        {
            _tenantId = tenantProvider.GetTenantId();
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="options"></param>
        /// <param name="mediator"></param>
        /// <param name="capBus"></param>
        public MessageContext(DbContextOptions options, IMediator mediator, ICapPublisher capBus) : base(options, mediator, capBus)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="modelBuilder"></param>
        ///
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var deletePropertyName = "IsDeleted";
            //租户
            var tenantIdPropertyName = "TenantId";
            //modelBuilder.Entity<Playlist>().HasQueryFilter(e => !e.IsDeleted);
            foreach (var item in modelBuilder.Model.GetEntityTypes())
            {
                var type = item.ClrType;
                var props = type.GetProperties().Where(c => c.IsDefined(typeof(DecimalPrecisionAttribute), true)).ToArray();
                foreach (var p in props)
                {
                    var precis = p.GetCustomAttribute<DecimalPrecisionAttribute>();
                    modelBuilder.Entity(type).Property(p.Name).HasColumnType($"decimal({precis.Precision},{precis.Scale})");
                }
                //实现IsDeleted==false 过滤
                if (typeof(ISoftDelete).IsAssignableFrom(type))
                {
                    var parameter = Expression.Parameter(type, "e");
                    var body = Expression.Equal(
                        Expression.Call(typeof(EF), nameof(EF.Property), new[] { typeof(bool) }, parameter, Expression.Constant(deletePropertyName)),
                        Expression.Constant(false));
                    modelBuilder.Entity(type).HasQueryFilter(Expression.Lambda(body, parameter));
                }
                //实现租户tenantId = _tenantId
                if (_tenantId > 0 && typeof(IMustHaveTenant).IsAssignableFrom(type))
                {
                    var parameter = Expression.Parameter(type, "e");
                    var body = Expression.Equal(
                        Expression.Call(typeof(EF), nameof(EF.Property), new[] { typeof(int) }, parameter, Expression.Constant(tenantIdPropertyName)),
                        Expression.Constant(_tenantId));
                    modelBuilder.Entity(type).HasQueryFilter(Expression.Lambda(body, parameter));
                }
            }

            base.OnModelCreating(modelBuilder);
            //创建索引
            modelBuilder.BuildIndexesFromAnnotations();
        }

        public virtual DbSet<Message.Domain.Message> Message { get; set; }
    }
}