using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WesleyCore.Domin.Abstractions;

namespace WesleyCore.User.Domain
{
    /// <summary>
    /// 角色权限
    /// </summary>
    [Table("RoleFeature", Schema = "System")]
    public class RoleFeature : Entity<long>
    {
        /// <summary>
        /// 构造
        /// </summary>
        protected RoleFeature()
        {
        }

        /// <summary>
        /// Feature主键
        /// </summary>
        public Guid FeatureId { get; set; }

        /// <summary>
        /// Role主键
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}