using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WesleyCore.Dtos;
using WesleyCore.User.Domain.Repository;
using WesleyUntity;

namespace WesleyCore.User.Application
{
    /// <summary>
    ///
    /// </summary>
    public class FeatureMenuTreeHendler : IRequestHandler<GetFeatureMenuTreeInput, List<Tree>>
    {
        /// <summary>
        /// 仓储
        /// </summary>
        private readonly IFeatureRepository _featureRepository;

        /// <summary>
        ///
        /// </summary>
        /// <param name="featureRepository"></param>
        public FeatureMenuTreeHendler(IFeatureRepository featureRepository)
        {
            _featureRepository = featureRepository;
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<Tree>> Handle(GetFeatureMenuTreeInput request, CancellationToken cancellationToken)
        {
            var menulist = await GetPersonalFeature(request);
            var isflat = false;//查询不展示树,全部都是根节点
            if (!request.Search.IsNullEmpty())
            {
                menulist = menulist.Where(q => PYMHelper.GetPinYinCode(q.Name).Contains(request.Search.Trim().ToUpper()) || q.Name
                .Contains(request.Search.Trim())).Where(a => !a.Url.IsNullEmpty()).ToList();
                isflat = true;
            }
            return BuildTree(menulist, isflat);
        }

        /// <summary>
        /// 构建树
        /// </summary>
        /// <param name="listFeature"></param>
        /// <param name="isFlat">查询不弄成树</param>
        /// <param name="roleFeatureIds">查询不弄成树</param>
        /// <returns></returns>
        private List<Tree> BuildTree(List<Domain.Feature> listFeature, bool isFlat = false, List<Guid> roleFeatureIds = null)
        {
            //根节点
            var rootMenus = listFeature.Where(m => m.IsRoot && !m.ParentId.HasValue).OrderBy(m => m.Sort);

            //最终组合节点
            var menus = new List<Tree>();

            if (isFlat)//查询
            {
                foreach (var menu in listFeature)
                {
                    var node = new Tree
                    {
                        Id = menu.Id,
                        Text = menu.Name,
                        Attributes = new { menu.Url, menu.PicUrl, menu.DOM, menu.ParentName, menu.Sort, menu.Memo, menu.IsHidden },
                        Checked = !menu.IsHidden
                    };
                    menus.Add(node);
                }
            }
            else
            {
                foreach (var menu in rootMenus)
                {
                    var node = new Tree
                    {
                        Id = menu.Id,
                        Text = menu.Name,
                        Attributes = new { menu.Url, menu.PicUrl, menu.DOM, menu.ParentName, menu.Sort, menu.Memo, menu.IsHidden },
                        Children = new List<Tree>()
                    };
                    if (roleFeatureIds == null)
                    {
                        node.Checked = !menu.IsHidden;
                    }
                    else
                    {
                        node.Checked = roleFeatureIds.Contains(menu.Id);
                    }
                    menus.Add(node);
                    GetTreeByParent(node, listFeature, roleFeatureIds);
                }
            }
            return menus;
        }

        /// <summary>
        /// 在所有结点中通过父节点获取其子节点
        /// </summary>
        /// <param name="pMenu"></param>
        /// <param name="menus"></param>
        /// <param name="roleFeatureIds">角色拥有的菜单</param>
        private void GetTreeByParent(Tree pMenu, List<Domain.Feature> menus, List<Guid> roleFeatureIds = null)
        {
            var pGuid = pMenu.Id;

            //在所有节点中找出当前节点的子节点
            var childrens = menus.Where(m => m.ParentId == pGuid).OrderBy(m => m.Sort);

            if (childrens != null && childrens.Any())
            {
                pMenu.State = roleFeatureIds == null ? "closed" : "open";
                foreach (var children in childrens)
                {
                    var menu = new Tree
                    {
                        Id = children.Id,
                        Text = children.Name,
                        Attributes = new { children.Url, children.PicUrl, children.DOM, children.ParentName, children.Sort, children.Memo, children.IsHidden },
                        Children = new List<Tree>()
                    };
                    if (roleFeatureIds == null)
                    {
                        menu.Checked = !children.IsHidden;
                    }
                    else
                    {
                        menu.Checked = roleFeatureIds.Contains(children.Id);
                    }
                    //将上层也置为未选中（checkbox: true, cascadeCheck: true）
                    if (menu.Checked == false)
                        pMenu.Checked = false;

                    pMenu.Children.Add(menu);
                    GetTreeByParent(menu, menus, roleFeatureIds);
                }
            }
        }

        /// <summary>
        /// 获取个人的菜单列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<List<Domain.Feature>> GetPersonalFeature(GetFeatureMenuTreeInput request)
        {
            if (request.IsAdmin)
            {
                return _featureRepository.GetAll().Where(f => f.StartDate <= DateTime.Now && f.ExpireDate >= DateTime.Now && !f.IsHidden && f.Type == FeatureTypeEnum.PC菜单).ToList();
            }
            else
            {
                return await _featureRepository.GetUserPCMenuFeatureList(request.UserId);
            }
        }
    }
}