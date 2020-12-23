using GeekTime.Infrastructure.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WesleyCore.Domin.Abstractions;

namespace WesleyCore.Infrastruction.Core
{
    /// <summary>
    /// 仓储实现
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    /// <typeparam name="TDbContext">数据库链接</typeparam>
    public abstract class Repository<TEntity, TDbContext> : IRepository<TEntity> where TEntity : Entity, IAggregateRoot where TDbContext : EFContext
    {
        /// <summary>
        /// 数据链接
        /// </summary>
        protected virtual TDbContext DbContext { get; set; }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="context"></param>
        public Repository(TDbContext context)
        {
            this.DbContext = context;
        }

        /// <summary>
        /// 工作单元
        /// </summary>
        public virtual IUnitOfWork UnitOfWork => DbContext;

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual TEntity Add(TEntity entity)
        {
            return DbContext.Add(entity).Entity;
        }

        /// <summary>
        /// 新增实体异部
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Add(entity));
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual TEntity Update(TEntity entity)
        {
            return DbContext.Update(entity).Entity;
        }

        /// <summary>
        /// 更新实体异部
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Update(entity));
        }

        /// <summary>
        /// 更新列表
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public IList<TEntity> UpdateList(IList<TEntity> entities)
        {
            foreach (var item in entities)
            {
                Update(item);
            }
            return entities;
        }

        /// <summary>
        /// 更新列表
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<IList<TEntity>> UpdateListAsync(IList<TEntity> entities, CancellationToken cancellationToken = default)
        {
            foreach (var item in entities)
            {
                Update(item);
            }
            return Task.FromResult(entities);
        }

        /// <summary>
        /// 移除实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual bool Remove(Entity entity)
        {
            DbContext.Remove(entity);
            return true;
        }

        /// <summary>
        /// 移除实体异部
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual Task<bool> RemoveAsync(Entity entity)
        {
            return Task.FromResult(Remove(entity));
        }

        /// <summary>
        /// 删除实体列表
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public bool RemoveList(IList<Entity> entities)
        {
            DbContext.RemoveRange(entities);
            return true;
        }

        /// <summary>
        /// 删除实体列表异部
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public Task<bool> RemoveListAsync(IList<Entity> entities)
        {
            DbContext.RemoveRange(entities);
            return Task.FromResult(true);
        }

        /// <summary>
        /// 根据表达式查询
        /// </summary>
        /// <param name="express"></param>
        /// <returns></returns>
        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> express)
        {
            Func<TEntity, bool> lamada = express.Compile();
            return DbContext.Set<TEntity>().Where(lamada).AsQueryable<TEntity>();
        }

        /// <summary>
        /// 获取整个集合
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> GetAll()
        {
            return DbContext.Set<TEntity>();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="express"></param>
        /// <returns></returns>
        public bool DeleteList(Expression<Func<TEntity, bool>> express)
        {
            Func<TEntity, bool> lamada = express.Compile();
            var lstEntity = DbContext.Set<TEntity>().Where(lamada);
            foreach (var entity in lstEntity)
            {
                DbContext.Remove(entity);
            }
            return true;
        }

        /// <summary>
        /// 获取第一个实体
        /// </summary>
        /// <param name="express"></param>
        /// <returns></returns>
        public TEntity FirstOrDefoult(Expression<Func<TEntity, bool>> express)
        {
            Func<TEntity, bool> lamada = express.Compile();
            return DbContext.Set<TEntity>().FirstOrDefault(lamada);
        }

        /// <summary>
        /// 获取第一个实体
        /// </summary>
        /// <param name="express"></param>
        /// <returns></returns>
        public Task<TEntity> FirstOrDefoultAsync(Expression<Func<TEntity, bool>> express)
        {
            Func<TEntity, bool> lamada = express.Compile();
            return Task.FromResult(DbContext.Set<TEntity>().FirstOrDefault(lamada));
        }
    }

    /// <summary>
    /// 仓储实现
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    /// <typeparam name="TKey">主键</typeparam>
    /// <typeparam name="TDbContext">数据库链接</typeparam>
    public abstract class Repository<TEntity, TKey, TDbContext> : Repository<TEntity, TDbContext>, IRepository<TEntity, TKey> where TEntity : Entity<TKey>, IAggregateRoot where TDbContext : EFContext
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="context"></param>
        public Repository(TDbContext context) : base(context)
        {
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public virtual bool Delete(TKey id)
        {
            var entity = DbContext.Find<TEntity>(id);
            if (entity == null)
            {
                return false;
            }
            DbContext.Remove(entity);
            return true;
        }

        /// <summary>
        /// 删除异部
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var entity = await DbContext.FindAsync<TEntity>(id, cancellationToken);
            if (entity == null)
            {
                return false;
            }
            DbContext.Remove(entity);
            return true;
        }

        /// <summary>
        /// 获取 通过key
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TEntity Get(TKey id)
        {
            return DbContext.Find<TEntity>(id);
        }

        /// <summary>
        /// 获取 通过key 异部
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return await DbContext.FindAsync<TEntity>(id, cancellationToken);
        }
    }
}