using MediatR;
using System.Runtime.CompilerServices;

namespace WesleyCore.Domin.Abstractions
{
    /// <summary>
    /// 领域事件接口
    /// </summary>
    public interface IDomainEvent : INotification
    {
    }
}