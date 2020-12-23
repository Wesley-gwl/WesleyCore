using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WesleyCore.Infrastruction.Core
{
    /// <summary>
    /// 工作单元
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// 保存修改实体
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 保存实体
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 允许领域事件
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task RunDomainEvent(CancellationToken cancellationToken = default);
    }
}