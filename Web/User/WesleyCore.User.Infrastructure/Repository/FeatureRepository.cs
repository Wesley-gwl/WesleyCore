using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WesleyCore.Infrastruction.Core;
using WesleyCore.Infrastructure;
using WesleyCore.User.Domain;
using WesleyCore.User.Domain.Repository;

namespace WesleyCore.User.Infrastructure
{
    /// <summary>
    /// 菜单权限仓储
    /// </summary>
    public class FeatureRepository : Repository<Feature, Guid, UserContext>, IFeatureRepository
    {
        /// <summary>
        ///
        /// </summary>
        private readonly UserContext _context;

        private readonly ITenantProvider _tenantProvider;

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tenantProvider"></param>
        public FeatureRepository(UserContext context, ITenantProvider tenantProvider) : base(context, tenantProvider)
        {
            _context = context;
            _tenantProvider = tenantProvider;
        }

        /// <summary>
        /// 获取用户菜单列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<Feature>> GetUserPCMenuFeatureList(Guid userId)
        {
            var feature = _context.Feature.Where(f => f.StartDate <= DateTime.Now && f.ExpireDate >= DateTime.Now && !f.IsHidden && f.Type == FeatureTypeEnum.PC菜单);

            return await (from or in _context.UserRole.Where(p => p.UserId == userId)
                          join rf in _context.RoleFeature on or.RoleId equals rf.RoleId
                          join f in feature on rf.FeatureId equals f.Id
                          select f).Distinct().ToListAsync();
        }
    }
}