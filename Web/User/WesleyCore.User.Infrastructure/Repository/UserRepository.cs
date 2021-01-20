using System;
using WesleyCore.Infrastruction.Core;
using WesleyCore.Infrastructure;
using WesleyCore.User.Domain;

namespace WesleyCore.User.Infrastructure.Repositories
{
    /// <summary>
    /// 用户仓储
    /// </summary>
    public class UserRepository : Repository<Domain.User, Guid, UserContext>, IUserRepository
    {
        private readonly UserContext _context;

        public UserRepository(UserContext context) : base(context)
        {
            _context = context;
        }
    }
}