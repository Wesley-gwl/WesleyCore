using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WesleyCore.Domin.Abstractions;

namespace WesleyCore.Infrastruction.Core
{
    public interface IRepository<TEntity, TKey> where TEntity : Entity<TKey>, IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }

        TEntity Add(TEntity entity);

        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    }
}