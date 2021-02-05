using Newtonsoft.Json;

namespace WesleyCore.Dtos
{
    /// <summary>
    /// 下拉框数据返回类
    /// </summary>
    public class Combo
    {
        /// <summary>
        /// id
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// value
        /// </summary>
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
    }
}