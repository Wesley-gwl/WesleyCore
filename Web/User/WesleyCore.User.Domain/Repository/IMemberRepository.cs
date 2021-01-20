using System.Threading.Tasks;
using WesleyCore.Infrastruction.Core;

namespace WesleyCore.User.Domain
{
    /// <summary>
    /// 会员仓储
    /// </summary>
    public interface IMemberRepository : IRepository<Member, int>
    {
        /// <summary>
        /// 获取会员以及过期时间
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        Task<Member> GetMemberShipInclude(int memberId);
    }
}