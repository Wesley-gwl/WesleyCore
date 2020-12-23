using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WesleyCore.Domain.Events;
using WesleyCore.Domin.Abstractions;

namespace WesleyCore.Domain.OrderAggregate
{
    [Table("Order")]
    public class Order : Entity<long>, IAggregateRoot, ISoftDelete, IMustHaveTenant
    {
        /// <summary>
        /// 保护
        /// </summary>
        protected Order()
        { }

        /// <summary>
        /// 用户主键
        /// </summary>
        [StringLength(20)]
        public string UserId { get; private set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [StringLength(20)]
        public string UserName { get; private set; }

        /// <summary>
        /// 地址
        /// </summary>
        public Address Address { get; private set; }

        /// <summary>
        /// 软删除
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? DeleteTime { get; set; }

        /// <summary>
        /// 租户
        /// </summary>
        public Guid TenantId { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="address"></param>
        public Order(string userId, string userName, Address address)
        {
            this.UserId = userId;
            this.UserName = userName;
            this.Address = address;
            this.AddDomainEvent(new OrderCreateDomainEvent(this));
        }

        /// <summary>
        /// 新增订单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public Order CreateOrder()
        {
            //新增事件
            this.AddDomainEvent(new OrderCreateDomainEvent(this));
            return this;
        }

        /// <summary>
        /// 修改地址
        /// </summary>
        /// <param name="address"></param>
        public void ChangeAddress(Address address)
        {
            this.Address = address;
            //修改地址
            //this.AddDomainEvent(new OrderAddressChangedDomainEvent(this));
        }
    }
}