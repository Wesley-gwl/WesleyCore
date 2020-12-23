using WesleyCore.Domain.OrderAggregate;
using WesleyCore.Infrastruction.Core;

namespace WesleyCore.Infrastruction.Repositories
{
    /// <summary>
    /// 仓储接口(可自定义)
    /// </summary>
    public interface IOrderRepository : IRepository<Order, long>
    {
    }
}