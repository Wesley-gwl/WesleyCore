using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WesleyCore.Infrastruction.Core;

namespace WesleyCore.Login.Infrastructure
{
    public class MemberRepository : Repository<Member, int, LoginContext>, IMemberRepository
    {
        private readonly LoginContext _context;

        public MemberRepository(LoginContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// 获取有效时间
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public async Task<Member> GetMemberShip(int memberId)
        {
            var member = await _context.Member
                .Include(b => b.MemberShip).Where(p => p.Id == memberId)
                .SingleOrDefaultAsync();
            member.MemberShip = member.MemberShip.Where(p => p.Status == MemberShipStatusEnum.有效).ToList();
            return member;
        }
    }
}