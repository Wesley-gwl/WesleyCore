using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WesleyCore.Infrastruction.Core.Extensions
{
    /// <summary>
    /// 实体反射帮组类
    /// </summary>
    public static class GenericTypeExtensions
    {
        /// <summary>
        /// 获取实体类型名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetGenericTypeName(this Type type)
        {
            var typeName = string.Empty;

            if (type.IsGenericType)
            {
                var genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name).ToArray());
                typeName = $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
            }
            else
            {
                typeName = type.Name;
            }

            return typeName;
        }

        /// <summary>
        /// 获取实体类型名称
        /// </summary>
        /// <param name="object"></param>
        /// <returns></returns>
        public static string GetGenericTypeName(this object @object)
        {
            return @object.GetType().GetGenericTypeName();
        }
    }
}