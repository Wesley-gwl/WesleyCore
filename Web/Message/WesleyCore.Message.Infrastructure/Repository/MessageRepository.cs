using System;
using System.Collections.Generic;
using System.Text;
using WesleyCore.Infrastruction.Core;
using WesleyCore.Infrastructure;
using WesleyCore.Message.Domain.Repository;

namespace WesleyCore.Message.Infrastructure.Repository
{
    public class MessageRepository : Repository<Domain.Message, Guid, MessageContext>, IMessageRepository
    {
        /// <summary>
        /// 链接
        /// </summary>
        private readonly MessageContext _context;

        /// <summary>
        /// 租户获取
        /// </summary>
        private readonly ITenantProvider _tenantProvider;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tenantProvider"></param>
        public MessageRepository(MessageContext context, ITenantProvider tenantProvider) : base(context, tenantProvider)
        {
            _context = context;
            _tenantProvider = tenantProvider;
        }
    }
}