using System;
using WesleyCore.Infrastruction.Core;

namespace WesleyCore.User.Domain
{
    public interface IUserRepository : IRepository<Domain.User, Guid>
    {
    }
}