using System;
using System.Collections.Generic;
using System.Text;
using WesleyCore.Domain.Aggregate.MemberAggregate;
using WesleyCore.EntityFrameworkCore;
using WesleyCore.Infrastruction.Core;

namespace WesleyCore.Core.Repositories
{
    public class MemberRepository : Repository<Member, int, MemberContext>, IMemberRepository
    {
        public MemberRepository(MemberContext context) : base(context)
        {
        }
    }
}