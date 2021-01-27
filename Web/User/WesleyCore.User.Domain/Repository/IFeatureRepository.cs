using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WesleyCore.Infrastruction.Core;

namespace WesleyCore.User.Domain.Repository
{
    /// <summary>
    /// 权限菜单
    /// </summary>
    public interface IFeatureRepository : IRepository<Feature, Guid>
    {
        /// <summary>
        /// 获取用户菜单列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<Feature>> GetUserPCMenuFeatureList(Guid userId);
    }
}