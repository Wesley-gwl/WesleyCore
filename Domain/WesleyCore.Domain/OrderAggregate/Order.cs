using System;
using WesleyCore.Domin.Abstractions;

namespace WesleyCore.Domain.OrderAggregate
{
    public class Order : Entity<long>, IAggregateRoot
    {
        public string UserId { get; set; }
        public string UserName { get; set; }

        public Order(string userId, string userName)
        {
            this.UserId = userId;
            this.UserName = userName;
        }
    }
}