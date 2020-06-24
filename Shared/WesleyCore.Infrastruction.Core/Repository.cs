using System;
using System.Collections.Generic;
using System.Text;

namespace WesleyCore.Infrastruction.Core
{
    public abstract class Repository<TEntity, Tkey, TDbContext> : Repository<TEntity, Tkey>
    {
        public Repository(TDbContext context) : base(context)
        {
        }
    }
}