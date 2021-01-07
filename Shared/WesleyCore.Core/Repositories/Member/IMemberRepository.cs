using System;
using System.Collections.Generic;
using System.Text;
using WesleyCore.Domain.Aggregate.MemberAggregate;
using WesleyCore.Infrastruction.Core;

namespace WesleyCore.Core.Repositories
{
    public interface IMemberRepository : IRepository<Member, int>
    {
    }
}