using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WesleyCore.Infrastruction.Core;
using WesleyCore.Infrastructure;
using WesleyCore.User.Domain;

namespace WesleyCore.User.Infrastructure
{
    public class MemberRepository : Repository<Member, int, UserContext>, IMemberRepository
    {
        private readonly UserContext _context;

        public MemberRepository(UserContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// 获取有效时间并查询
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public async Task<Member> GetMemberShipInclude(int memberId)
        {
            var member = await _context.Member
                .Include(b => b.MemberShip).Where(p => p.Id == memberId)
                .SingleOrDefaultAsync();
            member.MemberShip = member.MemberShip.Where(p => p.Status == MemberShipStatusEnum.有效).ToList();
            return member;
        }
    }
}