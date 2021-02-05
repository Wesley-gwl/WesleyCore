using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;
using WesleyCore.Domin.Abstractions;
using WesleyUntity;

namespace WesleyCore.User.Domain
{
    /// <summary>
    /// 菜单以及权限
    /// </summary>
    [Table("Feature", Schema = "System")]
    public class Feature : Entity<Guid>, ISoftDelete, IMustHaveTenant, IAggregateRoot
    {
        /// <summary>
        /// 构造
        /// </summary>
        protected Feature()
        {
        }

        /// <summary>
        /// 构造创建
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="picUrl"></param>
        /// <param name="dom"></param>
        /// <param name="isRoot"></param>
        /// <param name="isHidden"></param>
        /// <param name="url"></param>
        /// <param name="unionKey"></param>
        public Feature(Guid? parentId, string name, FeatureTypeEnum type, string picUrl, string dom, bool isRoot, bool isHidden, string url, Guid? unionKey, string memo)
        {
            Id = ComFunc.NewCombGuid();
            ParentId = parentId;
            Name = name;
            Type = type;
            PicUrl = picUrl;
            DOM = dom;
            IsRoot = isRoot;
            IsHidden = isHidden;
            Url = url;
            CreateTime = DateTime.Now;
            StartDate = DateTime.Now;
            ExpireDate = DateTime.Now.AddYears(10);
            UnionKey = unionKey;
            Memo = memo;
        }

        #region 字段

        /// <summary>
        /// 父类id
        /// </summary>
        public Guid? ParentId { get; private set; }

        /// <summary>
        /// 父类名称
        /// </summary>
        [StringLength(20)]
        public string ParentName { get; private set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Name { get; private set; }

        /// <summary>
        /// 功能类型
        /// </summary>
        [IndexColumn]
        public FeatureTypeEnum Type { get; private set; }

        /// <summary>
        /// 图片路径
        /// </summary>
        [StringLength(200)]
        public string PicUrl { get; private set; }

        /// <summary>
        /// dom元素绑定控件元素
        /// </summary>
        [StringLength(100)]
        public string DOM { get; private set; }

        /// <summary>
        /// 排序越小越优先
        /// </summary>
        public int? Sort { get; set; }

        /// <summary>
        /// 是否是根结点
        /// </summary>
        public bool IsRoot { get; private set; }

        /// <summary>
        /// 是否隐藏
        /// </summary>
        public bool IsHidden { get; private set; }

        /// <summary>
        /// 请求地址
        /// </summary>
        [StringLength(100)]
        public string Url { get; private set; }

        /// <summary>
        /// 菜单生效日期
        /// </summary>
        public DateTime StartDate { get; private set; }

        /// <summary>
        /// 菜单失效日期
        /// </summary>
        public DateTime ExpireDate { get; private set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; private set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; private set; }

        /// <summary>
        /// 关联平台菜单的ID，为空则表示租户自定义菜单
        /// </summary>
        public Guid? UnionKey { get; private set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200)]
        public string Memo { get; private set; }

        /// <summary>
        /// 备用
        /// </summary>
        public string Bak1 { get; set; }

        /// <summary>
        /// 备用
        /// </summary>
        public string Bak2 { get; set; }

        /// <summary>
        /// 备用
        /// </summary>
        public string Bak3 { get; set; }

        /// <summary>
        /// 备用
        /// </summary>
        public string Bak4 { get; set; }

        /// <summary>
        /// 备用
        /// </summary>
        public string Bak5 { get; set; }

        /// <summary>
        /// 删除
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        [IndexColumn]
        public int TenantId { get; set; }

        #endregion 字段

        #region 方法

        /// <summary>
        /// 更新菜单
        /// </summary>
        public void UpdateMenus(string name, string picUrl, bool isRoot, string url)
        {
            Name = name;
            PicUrl = picUrl;
            IsRoot = isRoot;
            Url = url;
        }

        /// <summary>
        /// 更新权限功能
        /// </summary>
        public void UpdateDOM(string name, string dom, string url)
        {
            Name = name;
            DOM = dom;
            Url = url;
        }

        /// <summary>
        /// 隐藏、显示菜单
        /// </summary>
        public void HiddenMenus(bool isHidden)
        {
            IsHidden = isHidden;
        }

        #endregion 方法
    }
}