using System;
using WesleyCore.Domin.Abstractions;

namespace WesleyCore.Domain.UserAggregate
{
    /// <summary>
    /// 用户聚合
    /// </summary>
    public class User : Entity<Guid>, IAggregateRoot
    {
    }
}