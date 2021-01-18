using System;
using System.Threading.Tasks;
using WesleyCore.Infrastruction.Core;
using WesleyCore.Login;

namespace WesleyCore.Login.Infrastructure
{
    /// <summary>
    /// 会员仓储
    /// </summary>
    public interface IMemberRepository : IRepository<Member, int>
    {
        Task<Member> GetMemberShip(int memberId);
    }
}