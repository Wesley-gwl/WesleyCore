using DotNetCore.CAP;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using WesleyCore.Domain.OrderAggregate;
using WesleyCore.Infrastruction.Repositories;

namespace WesleyCore.Application.Commands
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, long>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICapPublisher _capPublisher;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, ICapPublisher capPublisher)
        {
            _orderRepository = orderRepository;
            _capPublisher = capPublisher;
        }

        /// <summary>
        /// 接受
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<long> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            //var address = new Address(request.Street, request.City, request.ZipCode);
            //var order = new Order(request.UserId, request.UserName, address);
            //order.CreateOrder();
            //await _orderRepository.AddAsync(order);
            //await _orderRepository.UnitOfWork.RunDomainEvent(cancellationToken);
            var address = new Address("wen san lu", "hangzhou", "310000");
            var order = new Order("xiaohong1999", "xiaohong", address);

            _orderRepository.Add(order);
            await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return order.Id;
        }
    }
}