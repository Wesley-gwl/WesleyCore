using System;
using WesleyCore.Domin.Abstractions;

namespace WesleyCore.Domain.OrderAggregate
{
    public class Order : Entity<long>, IAggregateRoot
    {
        /// <summary>
        /// 用户主键
        /// </summary>
        public string UserId { get; private set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// 地址
        /// </summary>
        public Address Address { get; private set; }

        public Order(string userId, string userName)
        {
            this.UserId = userId;
            this.UserName = userName;
        }

        public override object[] GetKeys()
        {
            throw new NotImplementedException();
        }
    }
}