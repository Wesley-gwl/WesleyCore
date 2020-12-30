using System.Threading.Tasks;

namespace WesleyCore.Ordering.API.Application.IntegrationEvents
{
    /// <summary>
    ///
    /// </summary>
    public interface IOrderSubscriberService
    {
        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        Task OrderCreated(OrderCreatedIntegrationEvent @event);
    }
}