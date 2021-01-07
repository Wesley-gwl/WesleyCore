using DotNetCore.CAP;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations;
using WesleyCore.Configuration;
using WesleyCore.Infrastructure.Core;

namespace WesleyCore.EntityFrameworkCore
{
    /// <summary>
    /// 会员微服务数据链接
    /// </summary>
    public class MemberContext : EFContext
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="options"></param>
        /// <param name="mediator"></param>
        /// <param name="capBus"></param>
        public MemberContext(DbContextOptions options, IMediator mediator, ICapPublisher capBus) : base(options, mediator, capBus)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //软删除
            var deletePropertyName = "IsDeleted";
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
                if (type.GetProperty(deletePropertyName) != null)
                {
                    var parameter = Expression.Parameter(type, "e");
                    var body = Expression.Equal(
                        Expression.Call(typeof(EF), nameof(EF.Property), new[] { typeof(bool) }, parameter, Expression.Constant(deletePropertyName)),
                        Expression.Constant(false));
                    modelBuilder.Entity(type).HasQueryFilter(Expression.Lambda(body, parameter));
                }
            }

            base.OnModelCreating(modelBuilder);
            //创建索引
            modelBuilder.BuildIndexesFromAnnotations();
        }
    }
}