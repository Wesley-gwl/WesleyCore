using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WesleyCore.Domin.Abstractions
{
    /// <summary>
    /// 领域事件
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// 发送领域事件
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public Task PublishAsync(IDomainEvent domain);
    }
}