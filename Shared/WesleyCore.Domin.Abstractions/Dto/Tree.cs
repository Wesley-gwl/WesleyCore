using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WesleyCore
{
    /// <summary>
    /// EasyUI Tree 树
    /// </summary>
    public class Tree
    {
        public Tree()
        {
            // 默认值
            this.State = "open";
        }

        /// <summary>
        /// 绑定到节点的标识值
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        /// <summary>
        /// 要显示的节点文本
        /// </summary>
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        /// <summary>
        /// 节点状态，'open' 或 'closed'，默认是 'open'。
        /// <para>当设置为 'closed' 时，该节点有子节点，并且将从远程站点加载它们。</para>
        /// </summary>
        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        /// <summary>
        /// 指示节点是否被选中
        /// </summary>
        [JsonProperty(PropertyName = "checked")]
        public bool Checked { get; set; } = false;

        /// <summary>
        /// 给一个节点添加的自定义属性
        /// </summary>
        [JsonProperty(PropertyName = "attributes")]
        public object Attributes { get; set; }

        /// <summary>
        /// 定义了一些子节点的节点数组
        /// </summary>
        [JsonProperty(PropertyName = "children")]
        public ICollection<Tree> Children { get; set; }
    }

    public class Tree<T> : Tree
    {
        /// <summary>
        /// 定义了一些子节点的节点数组
        /// </summary>
        [JsonProperty(PropertyName = "children")]
        public new ICollection<Tree<T>> Children { get; set; }
    }
}