using WesleyCore.Domain.OrderAggregate;
using WesleyCore.EntityFrameworkCore;
using WesleyCore.Infrastruction.Core;

namespace WesleyCore.Infrastruction.Repositories
{
    /// <summary>
    /// 仓储
    /// </summary>
    public class OrderRepository : Repository<Order, long, DomainContext>, IOrderRepository
    {
        public OrderRepository(DomainContext context) : base(context)
        {
        }
    }
}