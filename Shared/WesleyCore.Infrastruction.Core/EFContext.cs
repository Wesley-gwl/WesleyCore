using DotNetCore.CAP;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;
using WesleyCore.Infrastruction.Core;
using WesleyCore.Infrastruction.Core.Extensions;

namespace WesleyCore.Infrastructure.Core
{
    public class EFContext : DbContext, IUnitOfWork, ITransaction
    {
        protected IMediator _mediator;
        private readonly ICapPublisher _capBus;

        public EFContext(DbContextOptions options, IMediator mediator, ICapPublisher capBus) : base(options)
        {
            _mediator = mediator;
            _capBus = capBus;
        }

        #region IUnitOfWork

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            //var entries = this.ChangeTracker.Entries();
            //var tenantIdPropertyName = "TenantId";
            ////添加租户id
            //foreach (var item in entries)
            //{
            //    if (item.State == EntityState.Added)
            //    {
            //        var type = item.Property(tenantIdPropertyName);
            //        if (type != null)
            //        {
            //            type.CurrentValue = GetTentantId();
            //        }
            //        //log 新增
            //    }
            //    else if (item.State == EntityState.Modified)
            //    {
            //        //更新日志
            //        var temp = item.Property("TenantId");
            //        //旧数据为
            //        //temp.OriginalValue;
            //        //修改后的数据为
            //        //temp.CurrentValue
            //        //更新前数据
            //    }
            //    else if (item.State == EntityState.Deleted)
            //    {
            //        //删除日志
            //    }
            //}
            await base.SaveChangesAsync(cancellationToken);
            await _mediator.DispatchDomainEventsAsync(this);
            return true;
        }

        /// <summary>
        /// 允许领域事件
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task RunDomainEvent(CancellationToken cancellationToken = default)
        {
            await _mediator.DispatchDomainEventsAsync(this);
        }

        #endregion IUnitOfWork

        #region ITransaction

        private IDbContextTransaction _currentTransaction;

        public IDbContextTransaction GetDbContextTransaction() => _currentTransaction;

        public bool HasActiveTransaction => _currentTransaction != null;

        public Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null) return null;
            _currentTransaction = Database.BeginTransaction(_capBus, autoCommit: false);
            return Task.FromResult(_currentTransaction);
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await SaveChangesAsync();
                transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        /// <summary>
        /// 回滚
        /// </summary>
        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        #endregion ITransaction
    }
}