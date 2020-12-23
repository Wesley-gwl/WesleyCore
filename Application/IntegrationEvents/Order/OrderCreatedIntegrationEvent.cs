namespace GeekTime.Ordering.API.Application.IntegrationEvents
{
    public class OrderCreatedIntegrationEvent
    {
        public OrderCreatedIntegrationEvent(long orderId) => OrderId = orderId;

        public long OrderId { get; }
    }
}