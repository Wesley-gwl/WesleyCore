using Newtonsoft.Json;

namespace WesleyCore
{
    /// <summary>
    /// 移动树时传入参数
    /// </summary>
    public class TreeOrder
    {
        [JsonProperty("source")]
        /// <summary>
        /// 放置的目标节点 id 属性
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 源节点 id 属性
        /// </summary>
        [JsonProperty("target")]
        public string Target { get; set; }

        /// <summary>
        /// 表示放置操作，可能的值是：'append'、'top' 或 'bottom'。
        /// </summary>
        [JsonProperty("point")]
        public TreeOrderPoint Point { get; set; }
    }

    public enum TreeOrderPoint
    {
        /// <summary>
        /// 插入
        /// </summary>
        [JsonProperty("append")]
        Append,

        /// <summary>
        /// 移动到目标节点上方
        /// </summary>
        [JsonProperty("top")]
        Top,

        /// <summary>
        /// 移动到目标节点下方
        /// </summary>
        [JsonProperty("bottom")]
        Bottom
    }
}