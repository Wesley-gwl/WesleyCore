using WesleyCore.Domain.OrderAggregate;
using WesleyCore.Infrastruction.Core;

namespace WesleyCore.Infrastruction
{
    public class DomainContext : EFContext
    {
        public DomainContext(DbContextOptions options, IMediator mediator)
        {
        }

        public DbSet<Order> Order { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating();
        }
    }
}