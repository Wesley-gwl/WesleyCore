using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WesleyCore.Domin.Abstractions;

namespace WesleyCore.Infrastruction.Core.Extensions
{
    public static class MediatorExtension
    {
        /// <summary>
        /// 注册领域事件
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, DbContext ctx)
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            domainEntities.ToList().ForEach(entity => entity.Entity.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
            {
                await mediator.Publish<INotification>(domainEvent);
            }
        }

        /// <summary>
        /// 注册领域事件
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static async Task DispatchDomainEventAsync(this IMediator mediator, IDomainEvent ctx)
        {
            await mediator.Publish<INotification>(ctx);
        }
    }
}