using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WesleyCore.Domin.Abstractions;
using WesleyCore.User.Domain.Events.Feature;
using WesleyCore.User.Domain.Repository;

namespace WesleyCore.User.Application.DomainEventHendlers.Feature
{
    /// <summary>
    /// 创建菜单
    /// </summary>
    public class CreateFeatureMenuDomainEventHandler : IDomainEventHandler<CreateFeatureMenuDomainEvent>
    {
        /// <summary>
        /// 仓储
        /// </summary>
        private readonly IFeatureRepository _featureRepository;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="featureRepository"></param>
        public CreateFeatureMenuDomainEventHandler(IFeatureRepository featureRepository)
        {
            _featureRepository = featureRepository;
        }

        /// <summary>
        /// 创建会员菜单
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(CreateFeatureMenuDomainEvent notification, CancellationToken cancellationToken)
        {
            //通过grpc调用管理平台的服务获取要新增菜单的列表
            //先写死
            var featureList = new List<Domain.Feature>
            {
                new Domain.Feature(null, "用户管理", FeatureTypeEnum.PC菜单, null, null, false, false, "url", null, "用户菜单"),
                new Domain.Feature(null, "客户管理", FeatureTypeEnum.PC菜单, null, null, false, false, "url", null, "客户管理"),
                new Domain.Feature(null, "菜单管理", FeatureTypeEnum.PC菜单, null, null, false, false, "url", null, "菜单管理")
            };
            var parentList = featureList.Where(p => p.ParentId == null).ToList();
            foreach (var item in parentList)
            {
                await UpdateAndSonList(item, featureList, notification.TenantId);
            }
        }

        /// <summary>
        /// 遍历更新
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="menuAdminList"></param>
        /// <param name="memberId"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        private async Task UpdateAndSonList(Domain.Feature menu, List<Domain.Feature> menuAdminList, int memberId, Guid? pid = null)
        {
            var sonList = menuAdminList.Where(p => p.ParentId == menu.Id).ToList();

            //先加自己
            await _featureRepository.InsertAndGetIdAsync(menu);

            if (sonList.Count > 0)
            {
                foreach (var item in sonList)
                {
                    await UpdateAndSonList(item, menuAdminList, memberId, menu.Id);
                }
            }
        }
    }
}