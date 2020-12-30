using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using WesleyCore.Domin.Abstractions;

namespace WesleyCore.Infrastruction.Core
{
    /// <summary>
    /// 仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> where TEntity : Entity, IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity Add(TEntity entity);

        /// <summary>
        /// 新增异部
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity Update(TEntity entity);

        /// <summary>
        /// 更新异部
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新列表
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        IList<TEntity> UpdateList(IList<TEntity> entities);

        /// <summary>
        /// 更新列表异部
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<TEntity>> UpdateListAsync(IList<TEntity> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Remove(Entity entity);

        /// <summary>
        /// 移除异部
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> RemoveAsync(Entity entity);

        /// <summary>
        /// 移除列表
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        bool RemoveList(IList<Entity> entities);

        /// <summary>
        /// 移除列表
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<bool> RemoveListAsync(IList<Entity> entities);

        /// <summary>
        /// 根据lamada表达式查询集合
        /// </summary>
        /// <param name="selector">lamada表达式</param>
        /// <returns></returns>
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> express);

        /// <summary>
        /// 获取整个集合
        /// </summary>
        /// <param name="selector">lamada表达式</param>
        /// <returns></returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// 根据lamada表达式删除对象
        /// </summary>
        /// <param name="selector"> lamada表达式 </param>
        /// <returns> 操作影响的行数 </returns>
        bool DeleteList(Expression<Func<TEntity, bool>> express);

        /// <summary>
        /// 获取默认第一个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity FirstOrDefoult(Expression<Func<TEntity, bool>> express);

        /// <summary>
        /// 获取默认第一个实体 异部
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> FirstOrDefoultAsync(Expression<Func<TEntity, bool>> express);
    }

    /// <summary>
    /// 仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IRepository<TEntity, TKey> : IRepository<TEntity> where TEntity : Entity<TKey>, IAggregateRoot
    {
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Delete(TKey id);

        /// <summary>
        /// 删除异部
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(TKey id, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity Get(TKey id);

        /// <summary>
        /// 获取异部
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default);
    }
}