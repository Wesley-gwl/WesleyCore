using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WesleyCore.Dtos
{
    /// <summary>
    /// EasyUI Datagrid Columns 属性
    /// </summary>
    public class Column
    {
        /// <summary>
        /// 字段
        /// </summary>
        [JsonProperty("field")]
        public string Field { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// 是否隐藏
        /// </summary>
        [JsonProperty("hidden")]
        public bool Hidden { get; set; }
    }
}